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
        name: "Test",
        selectedBot: "",
        bots: [],
        selectedProfiles: [],
        profiles: [],
        message: ""
        profileName: "",
        tweet: {
            text: "Test"
        },
        selectedBot: "",
    },
    methods: {
        addProfile: function () {
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
        removeProfile: function () {
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
        trainProfile: (async function () {
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
                alert("Training complete");
            else
                errorLogger(result);
        }),

        saveBot: function () {

            let bot = {};

            bot.name = this.name;
            bot.profiles = this.profiles;

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
                        tweetApp.updateLists();
                    } else {
                        errorLogger(result);
                    }
                });
        },

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
        
        updateLists: function () {
            this.loadProfiles();
            this.loadBots();
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