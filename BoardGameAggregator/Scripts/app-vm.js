function AppViewModel() {
    var self = this;

    self.boardGames = ko.observableArray([]);
    self.detailsGame = ko.observable(null);

    self.searchName = ko.observable("");
    self.searchResults = ko.observableArray([]);

    self.loadingGames = ko.observable(true);
    self.loadingSearchResults = ko.observable(false);

    self.visibleGames = ko.observableArray([]);
    self.sortBy = ko.observable("customName");
    self.sortAscending = ko.observable(true);
    self.filterBy = ko.observableArray([]);

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

    self.determineIfSaved = function (id) {
        var match = ko.utils.arrayFirst(self.boardGames(), function (game) {
            return game.bggId() == id;
        });

        return match != null;
    };

    self.lookUpGameList = function () {
        self.searchResults.removeAll();
        self.loadingSearchResults(true);

        $('#resultsModal').modal('show');

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
                console.log(error);
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

    self.changeSorting = function (column) {
        // If we are already sorting by that column, just change direction
        if (self.sortBy() == column) {
            self.sortAscending(!self.sortAscending());
        } else {
            self.sortBy(column);
            self.sortAscending(true);
        }
    }

    self.sort = function (sortBy) {
        self.changeSorting(sortBy);
        self.loadingGames(true);
        self.sortGames();
        self.loadingGames(false);
    };

    // TODO: Filtering is not hooked up to the UI. 
    // This function merely adds an arbitarary filter
    self.addFilter = function () {
        var criterion = new FilterCriterion("rank", "<", "50");
        self.filterBy.push(criterion);
    };

    self.filterGames = function () {
        self.visibleGames([]);

        // Copy the games, maintaining reference for underlying observables
        // but not for the list itself
        self.visibleGames(self.boardGames.slice(0));

        self.visibleGames.remove(function (item) {

            var filterOut = false;

            // Loops through all the criteria, flagging the game if it doesn't fit
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

    // Whenever the board games list is changed, resort and filter
    self.subscribeToChanges = function () {

        self.boardGames.subscribe(function () {
            self.loadingGames(true);

            self.filterGames();
            self.sortGames();

            self.loadingGames(false);
        });

    };

    self.loadGames = function () {
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

                // Subscribe after the intial list is loaded, so we don't resort/filter after every added game
                self.subscribeToChanges();
            },
            error: function (error) {
                console.log("Failure.");
                console.log(error);
            }
        });
    }

    // Start the application by loading the initial set of games
    self.loadGames();

}

var app = new AppViewModel();
ko.applyBindings(app);