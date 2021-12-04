//(
//    function (d, a, s, h, b, oa, rd) {
//        if (!d[b]) {
//            oa = a.createElement(s), oa.async = 1;
//            oa.src = h;
//            rd = a.getElementsByTagName(s)[0];
//            rd.parentNode.insertBefore(oa, rd);
//        }
//        d[b] = d[b] || {};
//        d[b].addDashboard = d[b].addDashboard || function (v) {
//            (d[b].list = d[b].list || []).push(v)
//        };
//    }
//)
//    (window, document, 'script',
//        'https://cdn-a.cumul.io/js/cumulio.min.js', 'Cumulio');

//Cumulio.addDashboard({
//    container: '#myDashboard'
//    , dashboardId: '<%= ViewData["dashboardId"] %>'
//    , key: '<%= ViewData["key"] %>'
//    , token: '<%= ViewData["token"] %>'
//});

export function loadCumulioDashboard(dashboardElement, properties) {
    alert(dashboardElement);
    Cumulio.addDashboard({
        container: dashboardElement,
        key: properties.authKey,
        token: properties.authToken
    });
    //dashboardElement.dashboardId = properties.dashboardId;
    //dashboardElement.authKey = properties.authKey;
    //dashboardElement.authToken = properties.authToken;
}