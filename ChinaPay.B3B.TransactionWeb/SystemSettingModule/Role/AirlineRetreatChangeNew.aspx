<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirlineRetreatChangeNew.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.AirlineRetreatChangeNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            border: 1px solid #cdcdcf;
            text-align: center;
            word-wrap: break-word;
            word-break: break-all;
        }
        .form table td.txt-l
        {
            text-align: left;
        }
        .min-type a
        {
            min-width:50px;
        }
        .phone
        {
            background: url(/images/phone.png) no-repeat;
            padding-left: 40px;
        }
        .obvious-red
        {
            border: 1px solid #A35956;
            color: #A35956;
            margin: 10px;
            padding: 3px 5px;
        }
        #airlines{ text-align:center;}
    </style>
<body>
    <form id="form1" runat="server">
    <div class="form">
        <h3 class="titleBg">
            各航空公司退废票规定
            <label class="obvious pad-l">
                （此规定仅供参考，最终以航空公司规定为准）</label>
            <a href="./AirlineUpgradeChange.aspx?ShowType=Upgrade" class="pad-l obvious-a">查看各航空公司升舱规定</a>
            <a href="./AirlineUpgradeChange.aspx?ShowType=Check-In" class="pad-l obvious-a">查看各航空公司网上值机</a>
        </h3>
        <div class="box pager clearfix min-type" style="height: auto;" id="airlines" runat="server">
        </div>
        <div class="clearfix">
            <span class="obvious-red fr">请注意：凡是客票政策中备注了不得退票的，退票只退税费。</span>
        </div>
        <table class='box' id="tableBox">
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdfAirlineCode" />
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script type="text/javascript">
    function getAirlines(code) {
        var parameter = JSON.stringify({ "code": code });
        sendPostRequest("/SystemSettingHandlers/SystemAirline.ashx/QueryAirlinesByAirlineCodeNew", parameter,
                function (item) {
                    var airlineContent = new Array();
                    var rowspan = item.RefundAndReschedulingDetail.length;
                    airlineContent.push("<tbody id='" + code + "'>");
                    airlineContent.push("<tr><td rowspan='" + (rowspan + 4) + "' class='w10' style='width:9%;'><h4 class='obvious'>" + item.ShortName + "</h4><p class='txt-l'>" + item.Condition + "</p></td>");
                    airlineContent.push("<td rowspan='2' class='w10 b'>舱位</td>");
                    airlineContent.push("<td colspan='2' class='b'>退票手续费</td>");
                    airlineContent.push("<td colspan='2' class='b'>变更手续费</td>");
                    airlineContent.push("<td rowspan='2' class='w10 b' style='width:7%;'>自愿签转</td>");
                    airlineContent.push("<td rowspan='2' class='w30 b'>备注</td></tr>");
                    airlineContent.push('<tr><td class="w10 obvious-b">航班起飞前</td><td class="w10 obvious">航班起飞后</td><td class="w10 obvious-b">航班起飞前</td><td class="w10 obvious">航班起飞后</td></tr>');
                    $.each(item.RefundAndReschedulingDetail, function (index, detil) {
                        airlineContent.push("<tr><td>" + detil.Bunks + "</td>");
                        airlineContent.push("<td>" + detil.ScrapBefore + "</td>");
                        airlineContent.push("<td>" + detil.ScrapAfter + "</td>");
                        airlineContent.push("<td>" + detil.ChangeBefore + "</td>");
                        airlineContent.push("<td>" + detil.ChangeAfter + "</td>");
                        airlineContent.push("<td>" + detil.Endorse + "</td>");
                        if (index == 0) { airlineContent.push("<td rowspan='" + rowspan + "'>" + item.Remark + "</td>"); }
                        airlineContent.push("</tr>");
                    });
                    airlineContent.push('<tr><td>废票规定</td><td colspan="6" class="txt-l">' + item.Scrap + '</td></tr>');
                    airlineContent.push('<tr><td>航空公司电话</td><td colspan="6" class="txt-l"><div class="phone obvious">' + item.AirlineTel + '</div></td></tr>');
                    airlineContent.push("</tbody>");
                    var strAirlineContent = airlineContent.join("");
                    $("#tableBox").append(strAirlineContent);
                }, function (e) {
                    if (e.status == 300) {
                        alert(JSON.parse(e.responseText));
                    } else {
                        alert(e.statusText);
                    }
                });
    }
    $(function () {
        var airlineCode = $("#hdfAirlineCode").val();
        var aAirlines = $("#airlines a");
        aAirlines.click(function () {
            aAirlines.removeClass("cur");
            var self = $(this);
            self.addClass("cur");
            var code = self.attr("value");
            $("#tableBox>tbody").hide();
            if (document.getElementById(code)) {
                $("#" + code).show();
            } else {
                getAirlines(code);
            }
            //控制页面高度
            var rightFrame = parent.document.getElementById("rightFrame");
            if (rightFrame) {
                rightFrame.height = document.body.scrollHeight;
            }
        });
        aAirlines.each(function () {
            if ($(this).attr("value") == airlineCode) {
                $(this).trigger("click");
            }
        });
    });
</script>