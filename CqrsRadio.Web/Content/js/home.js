var home = (function () {
    var deezer = {
        getLoginStatus: (model) => {
            DZ.getLoginStatus(function (response) {
                if (response.authResponse) {
                    var accessToken = response.authResponse.accessToken;
                    DZ.api('/user/me', function (response) {
                        if (response.error)
                            return;
                        model.isLogged(true);
                        model.accessToken(accessToken);
                        model.userId(response.id);
                        model.name(response.name);
                        model.email(response.email);
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
                            model.isLogged(true);
                            model.accessToken(accessToken);
                            model.userId(response.id);
                            model.name(response.name);
                            model.email(response.email);
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
        },
        createPlaylist: (model) => {
            $.post('/Login',
                {
                    accessToken: model.accessToken(),
                    userId: model.userId(),
                    nickname: model.name(),
                    email: model.email(),
                    playlistName: model.playlist()
                },
                function () {

                });
        }
    };
    var public = {
        info: () => console.log("Welcome home!"),
        model: {
            isLogged: ko.observable(false),
            accessToken: ko.observable(''),
            userId: ko.observable(''),
            name: ko.observable(''),
            email: ko.observable(''),
            playlist: ko.observable(''),
            init: () => deezer.getLoginStatus(public.model),
            login: deezer.login,
            createPlaylist: deezer.createPlaylist
        }
    };
    public.model.signinCss = ko.pureComputed(function() {
        return this.isLogged() ? 'd-none' : '';
    }, public.model);

    public.model.signoutCss = ko.pureComputed(function () {
        return this.isLogged() ? '' : 'd-none';
    }, public.model);

    ko.applyBindings(public.model);
    return public;
})();

$(document).ready(function () {
    home.model.init();
    home.info();
});
