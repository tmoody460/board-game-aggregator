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
            Id: self.id, Name: self.customName, Rating: self.customRating, Played: self.played, Owned: self.owned,
            Comments: self.comments
        }
        $.ajax({
            url: '/Home/SaveGame',
            type: 'POST',
            data: { "game": game },
            dataType: "json",
            success: function (data) { console.log("Saved."); },
            error: function(error) { console.log(error); }
        });
    }, this).extend({ throttle: 1000 });
}

function AppViewModel() {
    var self = this;

    self.boardGames = ko.observableArray([]);
    self.detailsGame = ko.observable(null);
    self.searchName = ko.observable("");

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

    self.loadGames = function(){
        console.log("Loading...");

        $.ajax({
            url: '/Home/GetBoardGames',
            type: "GET",
            data: { "page": "1" },
            dataType: "json",
            success: function (data) {
                console.log("Success!");
                var games = $.map(data, function (game, i) {
                    var game = new BoardGame(game.Id, game.Name, game.Played, game.Owned, game.Rating, game.Comments,
                        game.Info.Name, game.Info.Description, game.Info.MinPlayers, game.Info.MaxPlayers, game.Info.Rank,
                        game.Info.Rating, game.Info.NumRatings, game.Info.MinPlayingTime, game.Info.MaxPlayingTime,
                        game.Info.Link, game.Info.ImageLink, self);

                    self.boardGames.push(game);
                });               
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