//文本框中输入，下拉同步
function SelectCity(txtObj, dropId) {
    var DropDown = document.getElementById(dropId);

    var InputValue = txtObj.toUpperCase();
    var CityCode = "";
    var CitySpellAll = "";
    var CitySpellShort = "";

    for (var i = 0; i < DropDown.options.length; i++) {
        if (InputValue.length == 3) {
            CityCode = DropDown.options[i].text.substring(0, 3).toUpperCase();
        }

        //三字码
        if (InputValue == CityCode) {
            DropDown.options[i].selected = true;
            break;
        }

        CitySpellAll = DropDown.options[i].value.toUpperCase();
//        CitySpellShort = DropDown.options[i].text.substring(DropDown.options[i].text.lastIndexOf('-') + 1).toUpperCase();

        //简称
        if (InputValue == CitySpellShort) {
            DropDown.options[i].selected = true;
            break;
        }

        //全称
        if (InputValue == CitySpellAll) {
            DropDown.options[i].selected = true;
            break;
        }

        //全称包含输入文本
        if (InputValue.length > 3 && CitySpellAll.length >= InputValue.length && InputValue == CitySpellAll.substring(0, InputValue.length)) {
            DropDown.options[i].selected = true;
            break;
        }
    }
}

//下拉选择，文本输入框同步
function ShowCityInTextBox(ddl, txtId) {
    var nowText = ddl.options[ddl.selectedIndex].text;
    if (nowText != "") {
        document.getElementById(txtId).value = nowText.substring(0, 3);
    }
    else {
        document.getElementById(txtId).value = "";
    }
}

        