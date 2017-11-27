var wfLocalization = {
    CNTranslateDic: [{ Key: "Title1", Value: "开启对" }],
    ENTranslateDic: [{ Key: "Title1", Value: "开启对" }],
    getTranslateResult: function () {
        var langObj = wfLocalization.CNTranslateDic;
        if ((navigator.language || navigator.browserLanguage).toLowerCase() != "zh-cn") {
            langObj = wfLocalization.ENTranslateDic;
        }

        for (var i = 0; i < langObj.length; i++) {
            var lan = langObj[i];
            if (lan.Key == key) {
                return lan.Value;
            }
        }
    }
};
