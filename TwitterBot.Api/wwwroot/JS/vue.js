function errorLogger(error) {
    console.error("Error: ", error);
}

Array.prototype.Remove = function (obj) {
    let arr = [];
    this.forEach(function (element) {
        if (element !== obj)
            arr.push(element);
    });

    return arr;
}
Array.prototype.select = function (method) {
    let arr = [];
    for (let i = 0; i < this.length; i++) {
        arr.push(method(this[i]));
    }
    return arr;
}

const botApp = new Vue({
    el: "#botApp",
    data: {
        botName: "",
        profileAlgorithm: 1,
        wordAlgorithm: 1,
        selectedProfiles: [],

        selectedBot: "",
        bots: [],

        profileName: "",
        profiles: [],
        tweet: {
            "text": ""
        },
        isProfileNameTwitterHandle: "#EAEAEA",

        messages: [],
        progressProfile: "",
        progressProfileMax: "",
        progressTweets: "",
        progressTweetsMax: "",


    },
    computed: {
        progressProfileCounter: function() {
            if (this.progressProfileMax === 0)
                return 0;
            else
                return Math.round((
                        (this.progressProfile + (this.progressTweetsCounter / 100)) / this.progressProfileMax) *
                    100);
        },
        progressTweetsCounter: function() {
            if (this.progressTweetsMax === 0)
                return 0;
            else
                return Math.round((this.progressTweets / this.progressTweetsMax) * 100);
        },
    },
    created: function() {
        this.profiles = loadProfiles();
        this.bots = loadBots();
        this.debounceIsProfileNameTwitterHandle = _.debounce(this.IsProfileNameTwitterHandle, 500);
    },
    watch: {
        profileName: function(newProfileName, oldProfileName) {
            this.isProfileNameTwitterHandle = "#fdf99e";
            this.debounceIsProfileNameTwitterHandle();
        },
        selectedProfiles: function(newProfileList, oldProfileList) {
            for (let profile of newProfileList) {
                profile.occurrence = (1 / newProfileList.length) * 100;
            }
        }
    },
    methods: {
        addProfile: (async function() {
            let profile = {};
            profile.name = this.profileName;

            let result = await fetch("api/twitter",
                {
                    body: JSON.stringify(profile),
                    method: "POST",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });

            if (result.status === 200) {
                this.setErrormessage("Added profile " + profile.name);
                await this.loadProfiles();

                if (this.profiles.length === 1) {
                    this.setErrormessage("First profile added");

                    this.selectedProfiles = this.profiles;

                    await this.trainProgress();

                    if (this.bots === undefined || this.bots.length === 0) {
                        if (this.botName === "")
                            this.botName = "First bot";

                        this.saveBot();
                    }
                }

            } else {
                if (result.status === 404) {
                    this.setErrormessage("Could not find twitter handle");
                } else {
                    errorLogger(result);
                }
            }
        }),
        removeProfile: function() {
            for (let profile of this.selectedProfiles) {

                fetch("api/twitter/handle",
                        {
                            body: JSON.stringify(profile),
                            method: "DELETE",
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json'
                            }

                        }).then(result => {
                        if (result.status === 200) {
                            return result.json();
                        }

                        throw result;
                    }).then(result => {
                        if (result !== null && result !== undefined) {
                            this.loadProfiles();
                            this.setErrormessage("Remove complete");
                        }
                    })
                    .catch(errorLogger);
            }
        },
        trainProgress: (async function() {
            let list = this.selectedProfiles;

            this.progressProfile = 0;
            this.progressProfileMax = list.length;

            for (let i = 0; i < list.length; i++) {
                let profile = list[i];
                this.setErrormessage("Starting training of " + profile.name);

                this.progressTweetsMax = 0;
                this.setErrormessage("Loading tweets");

                let result = await fetch("api/twitter/traindata",
                    {
                        body: JSON.stringify(profile),
                        method: "POST",
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        }
                    });

                if (result.status === 200) {
                    this.setErrormessage("Loading complete");
                    let tweets = await result.json();

                    this.progressTweets = 0;
                    this.progressTweetsMax = tweets.length;

                    for (let j = 0; j < tweets.length; j++) {
                        tweet = tweets[j];
                        result = await fetch("api/twitter/trainwithtweet",
                            {
                                body: JSON.stringify({
                                    profile,
                                    tweet
                                }),
                                method: "POST",
                                headers: {
                                    'Accept': 'application/json',
                                    'Content-Type': 'application/json'
                                }
                            });
                        if (result.status === 200) {

                        } else {
                            this.setErrormessage(
                                "Something went wrong with the training. Please contact the systemadministrator");
                        }
                        this.progressTweets++;
                    }
                    this.setErrormessage(`Profile ${profile.name} trained`);
                } else {
                    this.setErrormessage("Loading tweets failed");
                }

                this.progressProfile++;
            }

            this.setErrormessage("Training complete");
            this.progressProfileMax = 0;

        }),

        saveBot: (async function() {

            if (this.botName === "") {
                this.setErrormessage("No botname assigned");
                return;
            }
            let bot = {};

            bot.name = this.botName;
            bot.profiles = this.selectedProfiles;

            this.setErrormessage("Saving bot " + bot.name);

            bot.profileAlgorithm = this.profileAlgorithm;

            bot.wordAlgorithm = this.wordAlgorithm;


            let result = await fetch("api/bot",
                {
                    body: JSON.stringify(bot),
                    method: "POST",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });

            if (result.status === 200) {
                await this.loadBots();
                this.setErrormessage("Bot added");
                this.selectedBot = this.bots.find(tempBot => tempBot.name === bot.name);

                this.generateTweet();
            } else {
                errorLogger(result);
            }
        }),
        removeBot: (async function() {
            let result = await fetch("api/bot",
                {
                    body: JSON.stringify(this.selectedBot),
                    method: "DELETE",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });
            if (result.status === 200) {
                this.updateLists();
            } else {

            }

        }),

        generateTweet: (async function() {
            let url = "api/bot/" + this.selectedBot.id;
            let result = await fetch(url);
            if (result.status === 200)
                this.tweet = await result.json();
        }),
        postTweet: (async function() {
            var result = await fetch("api/twitter/PostToTwitter",
                {
                    body: JSON.stringify(this.tweet),
                    method: "POST",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });
            if (result.status === 200)
                alert("Tweet posted");
        }),

        simpleToggle: function() {

        },
        advancedToggle: function() {

        },

        IsProfileNameTwitterHandle: (async function() {
            let handle = this.profileName;
            if (handle !== "") {
                let result = await fetch("api/heartbeat/twitterhandle",
                    {
                        body: JSON.stringify(handle),
                        method: "POST",
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        }
                    });
                if (result.status === 200) {
                    this.isProfileNameTwitterHandle = "#9efda2";
                } else if (result.status === 404) {
                    this.isProfileNameTwitterHandle = "#FF0000";
                } else {
                    this.isProfileNameTwitterHandle = "#e74949";
                }
            } else
                this.isProfileNameTwitterHandle = "#EAEAEA";

        }),
        
        updateLists: function() {
            this.loadProfiles();
            this.loadBots();
        },
        setErrormessage: function(str) {
            let message = {};
            message.text = str;
            this.messages.push(message);
        },
        removeMessage: function(message) {
            let index = this.messages.findIndex(element => {
                return element.text === message.text;
            });

            this.messages = this.messages.splice(index, 0);
        },

        loadProfiles: (async function () {
            this.profiles = await loadProfiles();
        }),
        loadBots: (async function () {
            this.bots = await loadBots();
        }),

    }
});

async function loadProfiles() {
    let result = await fetch("api/twitter");
    if (result.status === 200) {
        return await result.json();
    } else {
        errorLogger(result);
    }
}

async function loadBots() {
    let result = await fetch("api/bot");
    if (result.status === 200) {
        return await result.json();
    } else {
        errorLogger(result);
    }
}

botApp.updateLists();