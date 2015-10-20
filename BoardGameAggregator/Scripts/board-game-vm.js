function BoardGame(id, customName, played, owned, customRating, comments, bggName, descripion, minPlayers, 
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
    self.descripion = ko.observable(descripion);
    self.minPlayers = ko.observable(minPlayers);
    self.maxPlayers = ko.observable(maxPlayers);
    self.rank = ko.observable(rank);
    self.rating = ko.observable(rating);
    self.numRatings = ko.observable(numRatings);
    self.minPlayingTime = ko.observable(minPlayingTime);
    self.maxPlayingTime = ko.observable(maxPlayingTime);
    self.bggLink = ko.observable(bggLink);
    self.imageLink = ko.observable(imageLink);

    self.viewGame = function()
    {
        parent.viewOnly(true);
        parent.detailsGame(self);
        $('#detailsModal').modal('show');
    }

    self.editGame = function () {
        parent.viewOnly(false);
        $('#detailsModal').modal('show');
    }

    self.deleteGame = function () {
        console.log("Delete");
    }
}

function AppViewModel() {
    var self = this;

    self.boardGames = ko.observableArray([]);
    self.viewOnly = ko.observable(true);
    self.detailsGame = ko.observable(null);

    function loadGames(){
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

    loadGames();

}

ko.applyBindings(new AppViewModel());