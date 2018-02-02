<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShouyishezhiGlobal.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.ShouyishezhiGlobal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>收益设置</title>
</head>
<link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
<body>
    <form id="form1" runat="server">
    <div>
        <h3 class="titleBg">
            收益设置</h3>
        <div class="importantBox broaden">
            <p class="obvious">
                提示：本页所设置的规则将对您的所有用户生效，请认真核对后再提交！</p>
            <br />
            <p>
                全局收益设置就是对您所有的用户进行普通票返点扣除或特殊票加价销售的操作，设置后用户在平台购票时您将得到您所设置的交易利润。</p>
        </div>
        <label class="g-radiobox">
            <input type="radio" name="shouyi" value="0" id="radBuxianzi" checked="true" runat="server" />不对任何用户进行限制</label><br />
        <label class="g-radiobox">
            <input type="radio" name="shouyi" value="1" id="radFenzu" runat="server" />允许各用户组分别进行收益设置</label><br />
        <label class="g-radiobox">
            <input type="radio" name="shouyi" value="2" id="radQuanju" runat="server" />使用全局的统一收益设置</label><br />
        <div class="clearfix global" style="display: none;">
            <div class="fl" style="width: 510px;">
                <ul class="tag-nav clearfix">
                    <li class="cur" val="1"><a>本公司政策</a></li>
                    <li val="0"><a>同行政策</a></li>
                </ul>
                <div class="tag-content" style="padding: 10px;">
                    <div class="tag-pane">
                        <div class="clearfix">
                            <div class="fl" style="width: 500px;">
                                <div class="simple-box clearfix">
                                    <label class="title">
                                        限制航空公司：</label>
                                    <div class="simple-content check-box-list">
                                        <div id="hangkonggongsi" runat="server">
                                        </div>
                                        <span class="obvious">
                                            <input type="radio" id="chkquanxuan" name="gongsixuanzhe" value="0" /><label for="chkquanxuan">全选</label></span>
                                        <span class="obvious">
                                            <input type="radio" id="chkfanxuan" name="gongsixuanzhe" value="1" /><label for="chkfanxuan">反选</label></span>
                                    </div>
                                </div>
                                <div class="simple-box">
                                    <label class="title">
                                        扣点/返点类型：</label>
                                    <div class="simple-content">
                                        <p>
                                            <span class="pad-r">
                                                <label>
                                                    <input type="radio" checked="checked" id="radqujian" name="fandiankoudian" value="1" />区间扣点</label></span>
                                            <span class="pad-l pad-r">
                                                <label>
                                                    <input type="radio" id="radfandian" name="fandiankoudian" value="2" />统一返点</label></span>
                                        </p>
                                        <p class="obvious1">
                                            请注意，本栏目的设置仅对普通单程政策进行区间扣点/统一返点</p>
                                    </div>
                                </div>
                                <div class="simple-box qujiankoudian">
                                    <label class="title">
                                        区间扣点设置：</label>
                                    <div class="simple-content">
                                        <table>
                                            <tbody class="quyu_table" id="rangeItems">
                                                <tr>
                                                    <td class='rangeSerial'>
                                                        返点在
                                                    </td>
                                                    <td>
                                                        <input style='width: 25px' class='text rangeStart' disabled='disabled' value='0' />%(<span>含</span>)
                                                    </td>
                                                    <td>
                                                        至
                                                    </td>
                                                    <td>
                                                        <input style='width: 25px' class='text rangeEnd' value='100' />%(含)
                                                    </td>
                                                    <td>
                                                        值
                                                    </td>
                                                    <td>
                                                        <input style='width: 25px' class='text rangeValue' value='0' />%
                                                    </td>
                                                    <td>
                                                        <a class='addRange add'>+</a>
                                                    </td>
                                                    <td>
                                                        <a class='delRange reduce' style='display: none;'>-</a>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <p>
                                            <span class="obvious1">你最多可用设置5个返点区间扣点，若多个冲突时，则取扣点最低的区间设置</span></p>
                                    </div>
                                </div>
                                <div class="simple-box tongyifandian" style="display: none;">
                                    <label class="title">
                                        统一返点设置：</label>
                                    <div class="simple-content">
                                        <p>
                                            统一返点值
                                            <input type="text" class="text text-s" id="tongyifandian" />%
                                        </p>
                                    </div>
                                </div>
                                <div class="simple-box">
                                    <label class="title">
                                        加价设置：</label>
                                    <div class="simple-content">
                                        <p>
                                            每张票加价
                                            <input type="text" class="text text-s" id="price" />
                                            元进行出售
                                             <span class="obvious1">请注意，本栏目只对特殊政策进行加价</span>
                                        </p>
                                    </div>
                                </div>
                                <div class="simple-box last">
                                    <a class="simple-add-btn btnAdd">添加到右侧分组</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="margin-left: 520px; padding-top: 30px;">
                <div class="accumulate-box xianshi">
                    <h4>
                        双击即可编辑您添加的分组</h4>
                        <br />
                    <h5>
                        本公司政策设置</h5>
                    <div id="bengongsi" runat="server">
                    </div>
                        <br />
                    <h5>
                        同行政策设置</h5>
                    <div id="tonghang" runat="server">
                    </div>
                </div>
                <div class="simple-box last">
                    <label class="title">
                        备注：</label>
                    <div class="simple-content">
                        <textarea class="text" style="width: 300px; height: 80px;" id="remark" runat="server"></textarea>
                        <p class="obvious1">
                            备注本次操作理由，若无可留空</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="btns">
            <input type="button" id="btnSave" class="btn class1" value="提交" />
        </div>
    </div>
    <input type="hidden" id="hidRanges" />
    <asp:HiddenField runat="server" ID="hidShouyishezhi" />
    </form>
</body>
</html>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/selector.js" type="text/javascript"></script>
<script src="/Scripts/SystemSetting/shouyishezhi.js?20130521" type="text/javascript"></script>
<script src="/Scripts/SystemSetting/IncomeSet.js?20130521" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        if ($("input[type='radio'][name='shouyi']:checked").val() == "2") {
            $(".global").show();
        } else {
            $(".global").hide();
        }
        $("input[type='radio'][name='shouyi']").click(function () {
            if ($(this).val() == "2") {
                $(".global").show();
            } else {
                $(".global").hide();
            }
        });
        $("#btnSave").click(function () {
            if ($("input[type='radio'][name='shouyi']:checked").val() == "2" && $.trim($("#hidShouyishezhi").val()) == "") {
                alert("右侧还没添加过任何收益信息，请先添加。");
                return;
            }
            var actionUrl = "/SystemSettingHandlers/IncomeLimitGroup.ashx/InsertIncomeLimitGroupGobal";
            sendPostRequest(actionUrl, JSON.stringify({ "type": $("input[type='radio'][name='shouyi']:checked").val(), "str": $("#hidShouyishezhi").val(), "remark": $("#remark").val() }), function (e) {
                if (e == true) {
                    alert("设置成功");
                } else {
                    alert("设置失败");
                }
            }, function (e) {
                alert(e.responseText);
            });
        });

    });


</script>
