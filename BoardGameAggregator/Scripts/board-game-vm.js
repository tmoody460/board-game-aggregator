function BoardGame(id, customName, played, owned, customRating, comments, bggName, descripion, minPlayers, 
    maxPlayers, rank, rating, numRatings, minPlayingTime, maxPlayingTime, bggLink, imageLink) {
    var self = this;

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
}

function AppViewModel() {
    var self = this;

    self.boardGames = ko.observableArray([]);

    function loadGames(){
        console.log("Loading...");

        $.ajax({
            url: '/Home/GetBoardGames',
            type: "GET",
            data: { "page": "1" },
            dataType: "json",
            success: function (data) {
                console.log("Success!");
                console.log(data);
            },
            error: function () {
                console.log("Failure.")
            }
        });
    }

    loadGames();

}

ko.applyBindings(new AppViewModel());