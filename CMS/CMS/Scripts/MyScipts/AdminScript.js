function AdminViewModel() {
    var self = this;
    var user = {
        UserId: self.UserId,
        UserName: self.UserName,
        NewRole: self.NewRole

    }

   
    self.UserId = ko.observable(0);
    self.UserName = ko.observable('');
    self.NewRole = ko.observable('');

  

    self.UsersData = ko.observableArray([]);
    GetAllUsers();



    function GetAllUsers() {

        $.ajax({
            type: "GET",
            url: '/MyAdmin/GetAllUsers',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.UsersData(data); //Put the response in ObservableArray

            },
            error: function (error) {
                alert(error.status + "<--and--> " + error.statusText);
            }
        });
        //Ends Here
    }

    var rolemodel = function (data) {
        var self = this;
        self.id = ko.observable(data.id);
        self.Roleq = ko.observable(data.Role);
    };


    var slf = this;
    slf.selectedChoice = ko.observable();
    slf.rolels = ko.observableArray([
         new rolemodel({ id: "admin", Role: "admin" }),
         new rolemodel({ id: "reader", Role: "reader" }),
         new rolemodel({ id: "writer", Role: "writer" }),
         
    ]);



    self.Update = function () {
        $.ajax({
            type: "Post",
            url: '/MyAdmin/UpdateUser',
            data: { UserId: self.UserId(), UserName: self.UserName(), NewRole: self.selectedChoice() },
            success: function (data) {
                GetAllUsers(data);
            }
        });
    }

    self.getselecteduser = function (user) {
        self.UserId(user.UserId),
        self.UserName(user.UserName)
    

    };
}
ko.applyBindings(new AdminViewModel());