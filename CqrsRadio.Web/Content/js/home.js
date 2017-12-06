var home = (function () {
    var deezer = {
        getLoginStatus: (model) => {
            DZ.getLoginStatus(function (response) {
                if (response.authResponse) {
                    DZ.api('/user/me', function (response) {
                        if (response.error)
                            return;
                        model.loginText("Hi " + response.name);
                        model.isLogged(true);
                    });
                } else {
                    console.log('User cancelled login or did not fully authorize.');
                }
            });
        },
        login: (model) => {
            if (model.isLogged())
                deezer.logout(model);
            else {
                DZ.login(function (response) {
                    if (response.authResponse) {
                        var accessToken = response.authResponse.accessToken;
                        DZ.api('/user/me', function (response) {
                            model.loginText("Hi " + response.name);
                            model.isLogged(true);
                            $.post('/Login',
                                {
                                    accessToken: accessToken,
                                    userId: response.id,
                                    nickname: response.name,
                                    email: response.email,
                                    playlistName: "testPlaylist"
                                },
                                function() {

                                });
                        });
                    } else {
                        console.log('User cancelled login or did not fully authorize.');
                    }
                }, { perms: 'basic_access,email' });
            }
           
        },
        logout: (model) => {
            DZ.logout();
            model.loginText("Signin");
            model.isLogged(false);
        }
    };
    var public = {
        info: () => console.log("Welcome home!"),
        model: {
            isLogged: ko.observable(false),
            loginText: ko.observable('Signin'),
            init: () => deezer.getLoginStatus(public.model),
            login: deezer.login
        }
    };

    ko.applyBindings(public.model);
    return public;
})();

$(document).ready(function () {
    home.model.init();
    home.info();
});
