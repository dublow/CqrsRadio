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
            model.isLogged(false);
        },
        createPlaylist: (model) => {
            if (!model.playlist() || model.playlist().trim() === '')
                model.isValid(false);
            else {
                $.post('/Login',
                    {
                        accessToken: model.accessToken(),
                        userId: model.userId(),
                        nickname: model.name(),
                        email: model.email(),
                        playlistName: model.playlist()
                    },
                    function() {
                        model.isValid(true);
                    });
            }
        }
    };
    var public = {
        info: () => console.log("Welcome home!"),
        model: {
            isLogged: ko.observable(false),
            isValid: ko.observable(true),
            accessToken: ko.observable(''),
            userId: ko.observable(''),
            name: ko.observable(''),
            email: ko.observable(''),
            playlist: ko.observable(''),
            init: () => deezer.getLoginStatus(public.model),
            login: deezer.login,
            logout: deezer.logout,
            createPlaylist: deezer.createPlaylist
        }
    };
    public.model.loginCss = ko.pureComputed(function() {
        return this.isLogged() ? 'd-none' : '';
    }, public.model);

    public.model.logoutCss = ko.pureComputed(function () {
        return this.isLogged() ? '' : 'd-none';
    }, public.model);

    public.model.invalidModel = ko.pureComputed(function () {
        return this.isValid() ? '' : 'is-invalid';
    }, public.model);

    ko.applyBindings(public.model);
    return public;
})();

$(document).ready(function () {
    home.model.init();
    home.info();
});
