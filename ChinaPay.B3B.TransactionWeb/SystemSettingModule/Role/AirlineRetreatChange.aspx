<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirlineRetreatChange.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.AirlineRetreatChange" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>航空公司退废票规定</title>
    <link href="/Styles/icon/main.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
    <style type="text/css">
        .box a
        {
            margin: 10px;
        }
        table.box td
        {
            vertical-align: top;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <div class="">
        <h3 class="titleBg">
            各航空公司退废票规定
            <label class="obvious">
                （此规定仅供参考，最终以航空公司规定为准）</label>
        </h3>
        <div class="success-tips box" style="padding: 5px;" id="airlines_div" runat="server">
            <p class="pager" id="airlines" runat="server">
            </p>
        </div>
        <br />
        <div id="display_tip" runat="server">
            <table class='box'>
                <colgroup>
                    <col class='w20' />
                    <col class='w20' />
                    <col class='w20' />
                    <col class='w20' />
                    <col class='w20' />
                </colgroup>
                <tbody id="table_body" runat="server">
                    <tr>
                        <th class="box">
                            航空公司
                        </th>
                        <th class="box">
                            退票规定
                        </th>
                        <th class="box">
                            废票规定
                        </th>
                        <th class="box">
                            改签规定
                        </th>
                        <th class="box">
                            备注
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdfAirlineCode" />
    </form>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var airlineCode = ""; //Carrier
            $("#airlines a").live("click", function () {
                var id = $(this).attr("value");
                $(".table_tr").css("display", "none");
                $("#" + id).css("display", "");
                $(".table_tr td").removeClass("box");
                $("#" + id + " td").addClass("box");
                $(".pager a").removeClass("cur");
                $(this).addClass("cur");
                if ($.trim($("#" + id).html()) == "") {
                    get_table_tr(id);
                }
            });
            var airlineCode = $("#hdfAirlineCode").val();
            $("#airlines a").each(function () {
                if ($(this).attr("value") == airlineCode) {
                    $(this).trigger("click");
                }
            });
        });
        function get_table_tr(code) {
            sendPostRequest("/SystemSettingHandlers/SystemAirline.ashx/QueryAirlinesByAirlineCode", JSON.stringify({ "code": code }), function (result) {
                var str = "";
                $.each(result, function (i, item) {
                    str = "<tr id='" + code + "' class='table_tr'> <td class='box'> " + item.ShortName + "<br />联系电话：" + item.AirlineTel + " </td> <td class='box'>" + item.Refund + " </td>  <td class='box'>" + item.Scrap + "  </td> <td class='box'>" + item.Change + " </td> <td class='box'>" + item.Remark + "</td></tr>";
                });
                $("#table_body").append(str);
            }, function (e) {
                if (e.status == 300) {
                    alert(JSON.parse(e.responseText));
                } else {
                    alert(e.statusText);
                }
            });
        };
    </script>
</body>
</html>
