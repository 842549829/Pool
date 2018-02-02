<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommodityShowList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.IntegralCommodity.CommodityShowList" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>积分商城</title>
    <link rel="stylesheet" type="text/css" href="/Styles/register.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="smallbd1">
        <h3 class="titleBg">
            物品兑换</h3>
        <div class="box-a">
            尊敬的用户您好，您当前共有积分：<asp:Label ID="lblZong" runat="server" CssClass="obvious" />
            分&nbsp;&nbsp;&nbsp;可用积分：<asp:Label ID="lblKeYong" runat="server" CssClass="obvious" />
            分 <a href="/About/help.aspx?flag=jifenhuode" target="_blank" class="obvious-a">如何获得积分？</a>
            <br />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <a class="obvious-a" href="IntegralZengZhang.aspx">积分增长记录</a> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <a class="obvious-a" href="IntegralXiaoFei.aspx">积分消费记录</a>
        </div>
        <div id="goodsList">
            <h4 class="goodsTitle">
                商品列表：</h4>
            <ul>
                <%string FileWeb = System.Configuration.ConfigurationManager.AppSettings["FileWeb"];
                  var query_list = ChinaPay.B3B.Service.Commodity.CommodityServer.GetCommodityList(true, new ChinaPay.Core.Pagination { PageIndex = 1, PageSize = 100 });
                  foreach (var item in query_list)
                  { 
                %>
                <li>
                    <img src="<%= FileWeb + item.CoverImgUrl %>" title="<%=item.Remark %>" alt="<%=item.Remark %>" />
                    <span>
                        <%=item.CommodityName %></span>
                    <div class="goodsNumber">
                        共<%=item.StockNumber %>件 剩<%=item.SurplusNumber %>件</div>
                    <div class="pointPrice">
                        <%=item.NeedIntegral %>
                        分
                        <input type="button" class="pointBtn" value="申请兑换" onclick="ExchangeCommodity('<%=item.ID %>','<%=item.NeedIntegral %>','<%=FileWeb + item.CoverImgUrl %>','<%=item.SurplusNumber %>','<%=item.NeedIntegral %>','<%=item.CommodityName %>')" />
                    </div>
                </li>
                <% } %>
            </ul>
        </div>
    </div>
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divCommodity'}"></a>
    <div id="divCommodity" class="form layer" style="display: none; width: 600px;">
        <div id="tipExchange">
            <div class="goodsPic">
                <img id="imgshow" />
                <span id="commodityNameTitle"></span>
            </div>
            <div class="goodsOperate">
                <span class="operateTitle">您是否确定兑换该商品？</span> <span class="operateContent">商品名称：<span
                    id="commodityName"></span><br />
                    所需积分：<span id="integral"></span>分<br />
                    兑换数量：<input type="text" id="exChangeNumber" class="text text-box" />件 </span>
                <input type="button" id="btnOk" class="btn class1" value="确认兑换" />
                <input type="button" class="btn class2 close" value="取 消" />
            </div>
        </div>
        <div id="tipSuccess">
            <span class="operateTitle">恭喜，您的兑换申请已成功提交<br />
                客服会稍后与您联系，请保持您的手机畅通 </span><span class="operateContent">商品名称：<span id="commodityNameSpan"></span><br />
                    兑换数量：<span id="exchangenum"></span> 件</span><span class="return"><a href="IntegralXiaoFei.aspx">查看消费记录</a>
                        <a href="CommodityShowList.aspx">继续兑换</a> <a href="CommodityShowList.aspx">关闭</a>
                    </span>
        </div>
    </div>
    <label style="display: none;" id="number">
    </label>
    <label style="display: none;" id="lblId">
    </label>
    </form>
</body>
</html>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#tipSuccess").hide();
        $("#btnOk").click(function () {
            if ($("#exChangeNumber").val() == "") {
                alert("兑换数量不能为空，请重新填写需兑换数量");
                $("#exChangeNumber").focus();
                return;
            }
            if (parseInt($("#number").html()) < parseInt($("#exChangeNumber").val())) {
                alert("兑换数量超过商品库存剩余数量，请重新填写需兑换数量");
                $("#exChangeNumber").val($("#number").html());
                $("#exChangeNumber").focus();
                return;
            }
            if (parseInt($("#exChangeNumber").val()) <= 0) {
                alert("兑换数量不能为零，请重新填写需兑换数量");
                $("#exChangeNumber").val(1);
                $("#exChangeNumber").focus();
                return;
            }
            sendPostRequest("/IntegralCommodityHandler/Commodity.ashx/ExChangreCommosity", JSON.stringify({ "id": $("#lblId").html(), "exChangeNum": $("#exChangeNumber").val() })
            , function (e) {
                if (e) {
                    $("#tipExchange").hide();
                    $("#tipSuccess").show();
                    $("#exchangenum").html($("#exChangeNumber").val());
                    $("#commodityNameSpan").html($("#commodityName").html());
                }
            }, function (e) {
                if (e.status == 300) {
                    alert(JSON.parse(e.responseText));
                } else {
                    alert(e.statusText);
                }
            });
        });
    });

    function ExchangeCommodity(id, intergral, imgUrl, surplusNumber, needIntegral, commodityName) {
        if (parseInt($("#lblKeYong").text()) < parseInt(intergral)) {
            alert("你的积分不足，无法兑换   加油攒积分吧亲！");
            return;
        }
        $("#exChangeNumber").val(1);
        $("#imgshow").attr("src", imgUrl);
        $("#commodityNameTitle").html(commodityName);
        $("#commodityName").html(commodityName);
        $("#integral").html(needIntegral);
        $("#number").html(surplusNumber);
        $("#lblId").html(id);
        $("#tipExchange").show();
        $("#divOpcial").click();
    } 
</script>
