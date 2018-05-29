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

const botApp = new Vue({
    el: "#botApp",
    data: {
        botName: "",
        profileAlgorithm: "",
        wordAlgorithm: "",
        selectedProfiles: [],

        selectedBot: "",
        bots: [],

        profileName: "",
        profiles: [],
        tweet: {
            text: ""
        },

        message: {
            expires: new Date(2018, 05, 28),
            message: "test",
        },
        progressProfile: "",
        progressProfileMax: "",
        progressTweets: "",
        progressTweetsMax: "",


    },
    computed:
    {
        errorMessage: function() {
            return {
                showError: (Date.now() < this.message.expires),
                message: this.message.message
            };
        }
    },
    methods: {
        addProfile: function() {
            let profile = {};
            profile.name = this.profileName;

            fetch("api/twitter",
                {
                    body: JSON.stringify(profile),
                    method: "POST",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                }).then(result => {
                if (result.status === 200) {
                    this.loadProfiles();
                } else {
                    if (result.status === 404) {
                        alert("Could not find twitterhandle");
                    } else {
                        console.error("Error: ", result);
                    }
                }
            });

        },
        removeProfile: function() {
            for (let profile of this.selectedProfiles) {

                console.log("Remove profile id " + profile.id);

                fetch("api/twitter/handle",
                        {
                            body: JSON.stringify(this.profiles),
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
                            alert("Remove complete");
                        }
                    })
                    .catch(errorLogger);
            }
        },
        trainProfile: (async function() {
            this.message = "Training in progress...";
            let result = await fetch("api/twitter/train/",
                {
                    body: JSON.stringify(this.selectedProfiles),
                    method: "POST",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });

            if (result.status === 200)
                this.message = "";
            else {
                this.message = "Sum ting went wong";
                errorLogger(result);
            }
        }),
        trainProgress: (async function() {
            let list = this.selectedProfiles;

            this.progressProfile = 0;
            this.progressProfileMax = list.length;

            for (let i = 0; i < list.length; i++) {
                profile = list[i];

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
                    let tweets = await result.json();

                    this.progressTweetsMax = tweets.length;

                    for (let j = 0; j < tweets.length; j++) {
                        tweet = tweets[j];
                        result = await fetch("api/twitter/trainwithtweet",
                            {
                                body: JSON.stringify({
                                    profile: profile,
                                    tweet: tweet
                                }),
                                method: "POST",
                                headers: {
                                    'Accept': 'application/json',
                                    'Content-Type': 'application/json'
                                }
                            });
                        if (result.status === 200) {

                        }
                        this.progressTweets++;
                    }
                }
                this.progressProfile++;
            }

        }),

        saveBot: function() {
            
            if (this.botName === "") {
                console.log("No botname assigned");
                this.setErrormessage("No botname assigned");
                return;
            }
            let bot = {};

            bot.name = this.name;
            bot.profiles = this.profiles;

            if (this.profileAlgorithm === "") {
                bot.profileAlgorithm = 1;
            } else {
                bot.profileAlgorithm = this.profileAlgorithm;
            }

            if (this.wordAlgorithm === "") {
                bot.wordAlgorithm = 1;
            } else {
                bot.wordAlgorithm = this.wordAlgorithm;
            }

            fetch("api/bot",
                {
                    body: JSON.stringify(bot),
                    method: "POST",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                }).then(result => {
                if (result.status === 200) {
                    this.updateLists();
                } else {
                    errorLogger(result);
                }
            });
        },
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

        generateTweet: (async function () {
            let url = "api/bot/" + this.selectedBot.id;
            let result = await fetch(url);
            if (result.status === 200)
                this.tweet = await result.json();
        }),
        postTweet: (async function () {
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

        simpleToggle: function () {

        },
        advancedToggle: function () {

        },

        updateLists: function () {
            this.loadProfiles();
            this.loadBots();
        },
        setErrormessage: function (message) {
            let time = new Date(Date.now());
            
            time.setSeconds(time.getSeconds() + 10);
            this.message = { message: message, expires: time };
            console.log(this.message);
        },

        loadProfiles: (async function () {
            this.profiles = await loadProfiles();
        }),
        loadBots: (async function () {
            this.bots = await loadBots();
        }),

    },
    created: function () {
        this.profiles = loadProfiles();
        this.bots = loadBots();
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