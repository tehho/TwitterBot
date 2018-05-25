function errorLogger(error) {
    console.error("Error: ", error);
}

Array.prototype.select = function (method) {
    let arr = [];
    for (let i = 0; i < this.length; i++) {
        arr.push(method(this[i]));
    }
    return arr;
};

document.getElementById("twitter-remove-submit").addEventListener("click",
    function () {
        removeHandle();
    });

document.getElementById("twitterhandle-train-submit").addEventListener("click",
    function () {
        trainHandle();
    });

document.getElementById("twitterhandle-submit").addEventListener("click",
    function () {
        let name = document.getElementById("twitterhandle-name").value;
        if (name === "") {
            alert("Twittername not set?");
            return;
        }
        postTwitterHandle(name).then(result => {
            if (result)
                getTwitterHandles();
        });
    });

document.getElementById("bot-submit").addEventListener("click",
    function () {
        let bot = {};

        bot.name = document.getElementById("bot-name").value;
        if (bot.name === "") {
            alert("No name of bot?");
            return;
        }

        bot.profiles = getSelectedProfiles();
        console.log(bot);

        postBotOptions(bot).then(result => {
            if (result)
                getBotHandles();
        });
    });

document.getElementById("twitter-generate-submit").addEventListener("click",
    function () {
        generateTweet();
    });

document.getElementById("twitter-post-submit").addEventListener("click",
    function () {
        postTweet();
    });


function removeHandle() {
    let profiles = getAllSelectedOptions(document.getElementById("twitterhandle-existing-names")).select(option => {
        return { name: option.value };
    });

    fetch("api/twitter/handle",
        {
            body: JSON.stringify(profiles),
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
        }).catch(errorLogger)
        .then(result => {
            if (result !== null && result !== undefined) {
                getTwitterHandles();
                alert("Remove complete");
            }
        });
}

function trainHandle() {
    let profiles = getSelectedProfiles();

    return fetch("api/twitter/train/",
        {
            body: JSON.stringify(profiles),
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        }).then(result => {
            if (result.status === 200)
                return result;
            throw result;

        }).catch(errorLogger)
        .then(result => {
            if (result !== null && result !== undefined) {
                alert("Training complete");
            }
        });
}

function postTwitterHandle(name) {
    let profile = {};
    profile.name = name;
    return fetch("api/twitter",
        {
            body: JSON.stringify(profile),
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }

        })
        .then(result => {
            if (result.status === 200) {
                alert("Successfuly added");
                return true;
            } else
                errorLogger(result);
            return false;
        });
}

function getTwitterHandles() {

    fetch("api/twitter/").catch(errorLogger).then(result => {
        if (result.status === 200) {
            result.json().catch(errorLogger).then(setTwitterHandleExistingNames);
        } else {
            errorLogger(result);
        }
    }).catch(errorLogger);
}

function postBotOptions(botOptions) {
    let bot = {};
    bot.name = botOptions.name;
    bot.profiles = botOptions.profiles;

    return fetch("api/bot",
        {
            body: JSON.stringify(bot),
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        }).then(result => {
            if (result === 200) {
                alert("Successfuly added");
                return true;
            } else {
                errorLogger(result);
            }
            return false;
        });

}

function generateTweet() {
    let profiles = getAllSelectedOptions(document.getElementById("twitterhandle-existing-names")).select(option => {
        return { name: option.value };
    });

    var bot =
        console.log(JSON.stringify(profiles));
    fetch("api/bot/",
        {
            query: JSON.stringify(profiles),
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }

        }).then(result => {
            if (result.status === 200)
                return result.json();
            throw result;
        }).catch(errorLogger)
        .then(result => {
            console.log(result);
            let content = document.getElementById("tweet-content");
            content.innerHTML = "";
            if (result !== null && result !== undefined) {
                content.innerHTML = result;
            }
        });
}

function postTweet() {
    let tweet = {};
    tweet.text = document.getElementById("tweet-content").innerHTML;


    fetch("api/twitter/PostToTwitter",
        {
            body: JSON.stringify(tweet),
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });
}


function getSelectedProfiles() {
    return getAllSelectedOptions(document.getElementById("twitterhandle-existing-names"))
        .select(option => {
            return { name: option.value };
        });
}

function getBotOptions() {

    let bot = {};

    let e = document.getElementById("bot-settings-container");

    bot.id = e.options[e.selectedIndex].value;

    return bot;
}

function getAllSelectedOptions(select) {
    return [].slice.call(select.options).filter(option => option.selected);
}

function getBotHandles() {
    fetch("api/bot/").catch(errorLogger)
        .then(result => {
            if (result.status === 200) {
                result.json().catch(errorLogger).then(setBotHandles);
            } else {
                errorLogger(result);
            }
        });
}

function setBotHandles(list) {
    var content = document.getElementById("twitter-bots");
    content.innerHTML = "";

    if (list !== null && list !== undefined) {
        if (list.length > 0) {
            for (let bot of list) {
                let name = bot.name;
                let id = bot.id;

                let element = document.createElement("option");
                element.value = id;
                element.innerText = name;
                content.append(element);
            }
        }

    }
}

function setTwitterHandleExistingNames(list) {
    var content = document.getElementById("twitterhandle-existing-names");
    content.innerHTML = "";

    if (list !== null && list !== undefined) {
        if (list.length > 0) {

            for (let obj of list) {
                let name = obj.name;
                let element = document.createElement("option");
                element.value = name;
                element.innerText = name;
                content.append(element);
            }
        }
    }
}

getTwitterHandles();
//getBotHandles();