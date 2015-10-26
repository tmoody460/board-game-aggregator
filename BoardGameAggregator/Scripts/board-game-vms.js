function BoardGame(id, customName, played, owned, customRating, comments, bggName, description, minPlayers, 
    maxPlayers, rank, rating, numRatings, minPlayingTime, maxPlayingTime, bggLink, imageLink, bggId, parent) {
    var self = this;
    var parent = parent;

    self.id = ko.observable(id ? id : -1);
    self.customName = ko.observable(customName);
    self.played = ko.observable(played);
    self.owned = ko.observable(owned);
    self.customRating = ko.observable(customRating);
    self.comments = ko.observable(comments);
    self.bggName = ko.observable(bggName);
    self.description = ko.observable(description);
    self.minPlayers = ko.observable(minPlayers);
    self.maxPlayers = ko.observable(maxPlayers);
    self.rank = ko.observable(rank);
    self.rating = ko.observable(rating);
    self.numRatings = ko.observable(numRatings);
    self.minPlayingTime = ko.observable(minPlayingTime);
    self.maxPlayingTime = ko.observable(maxPlayingTime);
    self.bggLink = ko.observable(bggLink);
    self.imageLink = ko.observable(imageLink);
    self.redditLink = "https://www.reddit.com/r/boardgames/search?q=" + self.bggName() + "&restrict_sr=on";
    self.youtubeLink = "https://www.youtube.com/results?search_query=" + self.bggName() + " review";
    self.amazonLink = "https://www.amazon.com/s/ref=nb_sb_noss_2?url=search-alias%3Daps&field-keywords=" + self.bggName();
    self.bggId = ko.observable(bggId);

    self.validationErrors = ko.observableArray([]);
    self.previousCustomRating = ko.observable(customRating);
    self.previousCustomName = ko.observable(customName);

    self.viewGame = function()
    {
        parent.detailsGame(self);
        $('#detailsModal').modal('show');
    }

    self.editGame = function () {
        parent.detailsGame(self);
        $('#editModal').modal('show');

        // When the user closes the modal, if they gave bad inputs, revert
        $('#editModal').on('hidden.bs.modal', function () {
            if (self.validationErrors().length > 0) {
                self.customName(self.previousCustomName());
                self.customRating(self.previousCustomRating());
            }
        })
    }

    self.deleteGame = function () {
        parent.detailsGame(self);
        $('#deleteModal').modal('show');
    }

    self.deleteGameConfirmed = function () {
        $.ajax({
            url: '/Home/DeleteGame',
            type: 'POST',
            data: { "id": self.id },
            dataType: "json",
            success: function (data) {
                parent.boardGames.remove(function (item) {
                    return item.id() == self.id();
                });
            },
            error: function (error) { console.log(error); }
        });
    }

    self.validateInputs = function () {
        var valid = true;
        self.validationErrors.removeAll();

        if (self.customName().length > 80) {
            self.validationErrors.push("Brevity is the soul of wit. Choose a shorter name.");
            valid = false;
        } else if (self.customName().length == 0) {
            self.validationErrors.push("Please include a custom name. Or at least copy BGG's.");
            valid = false;
        }

        if (isNaN(self.customRating()) || self.customRating() === "") {
            self.validationErrors.push("Ratings must be numeric.");
            valid = false;
        } if (self.customRating() < 0 || self.customRating() > 10) {
            self.validationErrors.push("Ratings must be between 0 and 10. Inclusive.");
            valid = false;
        }

        return valid;
    };

    // Save game whenever its data fields change (if valid)
    ko.computed(function () {
        var valid = self.validateInputs();

        if (valid) {
            var game = {
                Id: self.id(), Name: self.customName(), Rating: self.customRating(), Played: self.played(), Owned: self.owned(),
                Comments: self.comments()
            }

            $.ajax({
                url: '/Home/SaveGame',
                type: 'POST',
                data: { "game": game },
                dataType: "json",
                success: function (data) { },
                error: function (error) { console.log(error); }
            });
        }
    }, this);

    // Store previous values of customName and customRating in case we need to revert
    // (in the case of invalid input)
    self.customName.subscribe(function (previousValue) {
        if (previousValue.length < 80 && previousValue.length > 0) {
            self.previousCustomName(previousValue);
        } 
    }, this, "beforeChange");

    self.customRating.subscribe(function (previousValue) {
        if (previousValue < 0 || previousValue > 10 && !isNaN(previousValue) && previousValue !== "") {
            self.previousCustomRating(previousValue);
        }
    }, this, "beforeChange");

}

function FilterCriterion(field, symbol, value)
{
    var self = this;

    self.field = field;
    self.isEqualTo = false;
    self.isGreaterThan = false;
    self.isLessThan = false;
    self.isNotEqualTo = false;
    self.value = value;

    if(symbol == "==")
    {
        self.isEqualTo = true;
    }else if(symbol == ">"){
        self.isGreaterThan = true;
    }else if(symbol == "<"){
        self.isLessThan = true;
    } else {
        self.isNotEqualTo = true;
    }
}

function SearchResult(name, id, vm) {

    var self = this;

    self.name = name;
    self.id = id;

    // Whenever the board game list updates, update the saved field in this search result
    // (e.g. add a game, the check mark should appear)
    self.saved = ko.computed(function () {
        vm.boardGames();
        return vm.determineIfSaved(id);
    });
}