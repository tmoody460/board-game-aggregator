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

    self.inputValidated = ko.observable(true);
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

    ko.computed(function () {
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
    self.saved = ko.computed(function () {
        vm.boardGames();
        return vm.determineIfSaved(id);
    });

    
}

function AppViewModel() {
    var self = this;

    self.boardGames = ko.observableArray([]);
    self.detailsGame = ko.observable(null);

    self.searchName = ko.observable("");
    self.searchResults = ko.observableArray([]);

    self.loadingGames = ko.observable(true);
    self.loadingSearchResults = ko.observable(false);

    self.sortBy = ko.observable("customName");
    self.sortAscending = ko.observable(true);
    self.filterBy = ko.observableArray([]);
    self.visibleGames = ko.observableArray([]);

    self.addGame = function (game) {
        $.ajax({
            url: '/Home/AddGame',
            type: 'POST',
            data: { "id": game.id },
            dataType: "json",
            success: function (data) {

                if (!data.Id) {
                    // Error message
                    console.log(data);
                } else {
                    // Valid data
                    var game = new BoardGame(data.Id, data.Name, data.Played, data.Owned, data.Rating, data.Comments,
                        data.Info.Name, data.Info.Description, data.Info.MinPlayers, data.Info.MaxPlayers, data.Info.Rank,
                        data.Info.Rating, data.Info.NumRatings, data.Info.MinPlayingTime, data.Info.MaxPlayingTime,
                        data.Info.Link, data.Info.ImageLink, data.Info.BggId, self);
                    self.boardGames.push(game);
                }

            },
            error: function (error) { console.log(error); }
        });
    }

    self.lookUpGame = function () {
        $.ajax({
            url: '/Home/LookUpGame',
            type: "GET",
            data: { "name": self.searchName() },
            dataType: "json",
            success: function (data) {
                if (!data.Id) {
                    // Error message
                    console.log(data);
                } else {
                    // Valid data
                    var game = new BoardGame(data.Id, data.Name, data.Played, data.Owned, data.Rating, data.Comments,
                        data.Info.Name, data.Info.Description, data.Info.MinPlayers, data.Info.MaxPlayers, data.Info.Rank,
                        data.Info.Rating, data.Info.NumRatings, data.Info.MinPlayingTime, data.Info.MaxPlayingTime,
                        data.Info.Link, data.Info.ImageLink, data.Info.BggId, self);
                    self.boardGames.push(game);
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    };

    self.determineIfSaved = function (id) {
        var match = ko.utils.arrayFirst(self.boardGames(), function (game) {
            return game.bggId() == id;
        });

        return match != null;
    };

    self.lookUpGameList = function () {
        $('#resultsModal').modal('show');

        self.searchResults.removeAll();
        self.loadingSearchResults(true);

        $.ajax({
            url: '/Home/LookUpGameList',
            type: "GET",
            data: { "name": self.searchName() },
            dataType: "json",
            success: function (data) {
                self.loadingSearchResults(false);
                var results = $.map(data, function (name, id) {
                    var result = new SearchResult(name, id, self);
                    self.searchResults.push(result);
                });
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });
    }


    self.sortGames = function () {
        var searchCriterion = self.sortBy();
        var ascending = self.sortAscending();

        var sortedArray = self.visibleGames.sort(
            function (left, right) {
                // If it has no value, list it at the end of the results
                if (left[searchCriterion]() === 0) {
                    return 1;
                } else if (right[searchCriterion]() === 0) {
                    return -1;
                }

                if (ascending) {
                    return left[searchCriterion]() == right[searchCriterion]() ? 0
                        : (left[searchCriterion]() < right[searchCriterion]() ? -1 : 1);
                } else {
                    return left[searchCriterion]() == right[searchCriterion]() ? 0
                        : (left[searchCriterion]() < right[searchCriterion]() ? 1 : -1);
                }
            });

        self.visibleGames(sortedArray);
    }

    self.addFilter = function () {
        var criterion = new FilterCriterion("rank", "!=", "1");
        self.filterBy.push(criterion);
    };

    self.changeSorting = function (column) {
        if (self.sortBy() == column) {
            self.sortAscending(!self.sortAscending());
        } else {
            self.sortBy(column);
            self.sortAscending(true);
        }
    }

    self.filterGames = function () {
        self.visibleGames([]);

        // Copy the games, maintaining reference for underlying observables
        // but not for the list itself
        self.visibleGames(self.boardGames.slice(0));

        self.visibleGames.remove(function (item) {

            var filterOut = false;

            ko.utils.arrayForEach(self.filterBy(), function (criterion) {
                if (criterion.isEqualTo && item[criterion.field]() != criterion.value) {
                    filterOut = true;
                } else if (criterion.isGreaterThan && item[criterion.field]() <= criterion.value) {
                    filterOut = true;
                } else if (criterion.isLessThan && item[criterion.field]() >= criterion.value) {
                    filterOut = true;
                } else if (criterion.isNotEqualTo && item[criterion.field]() == criterion.value) {
                    filterOut = true;
                }
            });

            return filterOut;
        });
    };

    self.sort = function (sortBy) {
        self.changeSorting(sortBy);
        self.loadingGames(true);
        self.sortGames();
        self.loadingGames(false);
    };

    self.subscribeToChanges = function () {

        self.boardGames.subscribe(function () {
            self.loadingGames(true);

            self.filterGames();
            self.sortGames();

            self.loadingGames(false);
        });

    };

    self.loadGames = function(){
        self.loadingGames(true);

        $.ajax({
            url: '/Home/GetBoardGames',
            type: "GET",
            data: { "page": "1" },
            dataType: "json",
            success: function (data) {

                var games = $.map(data, function (game, i) {
                    var game = new BoardGame(game.Id, game.Name, game.Played, game.Owned, game.Rating, game.Comments,
                        game.Info.Name, game.Info.Description, game.Info.MinPlayers, game.Info.MaxPlayers, game.Info.Rank,
                        game.Info.Rating, game.Info.NumRatings, game.Info.MinPlayingTime, game.Info.MaxPlayingTime,
                        game.Info.Link, game.Info.ImageLink, game.Info.BggId, self);

                    self.boardGames.push(game);
                });

                self.filterGames();
                self.sortGames();

                self.loadingGames(false);

                self.subscribeToChanges();
            },
            error: function (error) {
                console.log("Failure.");
                console.log(error);
            }
        });

    }
    
    self.loadGames();

}

ko.applyBindings(new AppViewModel());