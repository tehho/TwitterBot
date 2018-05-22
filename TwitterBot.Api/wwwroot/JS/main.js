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

document.getElementById("twitterhandle-submit").addEventListener("click", function () {
    let name = document.getElementById("twitterhandle-name").value;
    if (name === "") {
        alert("Name not set?");
        return;
    }
    postTwitterHandle(name);

    getTwitterHandles();
});

document.getElementById("twitter-generate-submit").addEventListener("click", function () {
    generateTweet();
});

document.getElementById("twitter-remove-submit").addEventListener("click", function () {
    removeHandle();
});

function postTwitterHandle(name) {
    let profile = {};
    profile.name = name;
    fetch("api/twitter",
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
                getTwitterHandles();
                return;
            }
            throw (result);
        }).catch(errorLogger);
}

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

function generateTweet() {
    let profiles = getAllSelectedOptions(document.getElementById("twitterhandle-existing-names")).select(option => {
        return { name: option.value };
    });
    console.log(JSON.stringify(profiles));
    fetch("api/twitter/tweet",
        {
            body: JSON.stringify(profiles),
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

            let content = document.getElementById("tweet-content");
            content.innerHTML = "";
            if (result !== null && result !== undefined) {
                content.innerHTML = result;
            }
        });
}

function getAllSelectedOptions(select) {
    return [].slice.call(select.options).filter(option => option.selected);
}

function getTwitterHandles() {

    fetch("api/twitter/").then(result => {
        if (result.status === 200) {
            result.json().catch(errorLogger).then(setTwitterHandleExistingNames);
        }
        else {
            errorLogger(result);
        }
    }).catch(errorLogger);
}

function setTwitterHandleExistingNames(list) {
    var content = document.getElementById("twitterhandle-existing-names");
    content.innerHTML = "";
    if (list.length > 0) {

        for (var obj of list) {
            let name = obj.name;
            let element = document.createElement("option");
            element.value = name;
            element.innerText = name;
            content.append(element);
        }
    }
}

getTwitterHandles();