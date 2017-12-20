var home = (function () {
    var loadMe = (response, model) => {
        if (response.authResponse) {
            var accessToken = response.authResponse.accessToken;
            DZ.api('/user/me', function (response) {
                if (response.error)
                    return;
                $.get('/CanCreatePlaylist/' + response.id,
                        function (responsePlaylist) {
                            model.canCreatePlaylist(responsePlaylist.data.canCreatePlaylist);
                            model.isLogged(true);
                            model.accessToken(accessToken);
                            model.userId(response.id);
                            model.name(response.name);
                            model.email(response.email);
                        })
                    .fail(function (response) {
                        model.isValid(false);
                        model.errorMessage(response.statusText);
                    });

                
            });
        } else {
            console.log('User cancelled login or did not fully authorize.');
        }
    };
    var deezer = {
        getLoginStatus: (model) => {
            DZ.getLoginStatus(function (response) {
                loadMe(response, model);
            });
        },
        login: (model) => {
            if (model.isLogged())
                deezer.logout(model);
            else {
                DZ.login(function (response) {
                    loadMe(response, model);
                }, { perms: 'basic_access,email,manage_library' });
            }
           
        },
        logout: (model) => {
            DZ.logout();
            model.isLogged(false);
            model.canCreatePlaylist(true);
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
                    function(response) {
                        model.isValid(response.isSuccess);
                        model.errorMessage('');
                        model.playlist('');
                        if (response.data.playlistCreated) 
                            model.canCreatePlaylist(false);
                    })
                    .fail(function (response) {
                        model.isValid(false);
                        model.errorMessage(response.statusText);
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
            errorMessage: ko.observable(''),
            canCreatePlaylist: ko.observable(true), 
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

    public.model.logoutAndPlaylistCss = ko.pureComputed(function () {
        return this.isLogged() && this.canCreatePlaylist() ? '' : 'd-none';
    }, public.model);

    public.model.invalidModel = ko.pureComputed(function () {
        return this.isValid() ? '' : 'is-invalid';
    }, public.model);

    public.model.playlistCss = ko.pureComputed(function () {
        return this.canCreatePlaylist() ? 'd-none' : '';
    }, public.model);

    ko.applyBindings(public.model);
    return public;
})();

$(document).ready(function () {
    home.model.init();
    home.info();
});
