function BoardGame(id, customName, played, owned, customRating, comments, bggName, description, minPlayers, 
    maxPlayers, rank, rating, numRatings, minPlayingTime, maxPlayingTime, bggLink, imageLink, parent) {
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

    self.viewGame = function()
    {
        parent.detailsGame(self);
        $('#detailsModal').modal('show');
    }

    self.editGame = function () {
        parent.detailsGame(self);
        $('#editModal').modal('show');
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
            success: function (data) { parent.boardGames.remove(function (item) { return item.id() == self.id() }) },
            error: function (error) { console.log(error); }
        });
    }

    ko.computed(function () {
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
            error: function(error) { console.log(error); }
        });
    }, this).extend({ throttle: 1000 });
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

function AppViewModel() {
    var self = this;

    self.boardGames = ko.observableArray([]);
    self.detailsGame = ko.observable(null);

    self.searchName = ko.observable("");
    self.searchResults = ko.observableArray([]);

    self.loadingGames = ko.observable(true);
    self.loadingSearchResults = ko.observable(false);

    self.sortBy = ko.observable("rank");
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
                        data.Info.Link, data.Info.ImageLink, self);
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
                if (!data.Id)
                {
                    // Error message
                    console.log(data);
                } else {
                    // Valid data
                    var game = new BoardGame(data.Id, data.Name, data.Played, data.Owned, data.Rating, data.Comments,
                        data.Info.Name, data.Info.Description, data.Info.MinPlayers, data.Info.MaxPlayers, data.Info.Rank,
                        data.Info.Rating, data.Info.NumRatings, data.Info.MinPlayingTime, data.Info.MaxPlayingTime,
                        data.Info.Link, data.Info.ImageLink, self);
                    self.boardGames.push(game);
                }                
            },
            error: function (error) {
                console.log(error);
            }
        });
    }

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
                    self.searchResults.push({ "name": name, "id": id });
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
                        game.Info.Link, game.Info.ImageLink, self);

                    self.boardGames.push(game);
                });

                self.filterGames();
                self.sortGames();

                self.loadingGames(false);
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