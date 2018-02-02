<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundAndReschedulingDetati.aspx.cs"
    Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.RefundAndReschedulingDetati" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>退改签规定维护</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>»<span>详细信息维护</span>
        </div>
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                            <tr>
                                <th>
                                   输入舱位：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtBunks" runat="server" CssClass="input5"></asp:TextBox>多个舱位以/隔开
                                </td>
                            </tr>
                            <tbody id="scrapTitle">
                            <tr>
                                <th>
                                   退票规定：
                                </th>
                                <td colspan="3">
                                    <textarea id="txtScrap" runat="server" class="input5" rows="3" cols="40"></textarea>
                                    <input type="button" id="btnSubScrap" style="width:120px" class="button" value="离站前后分别设置" />
                                </td>
                            </tr>
                            </tbody>
                            <tbody id="scrap" style="display:none">
                           <tr>
                                <th>
                                   离站前退票规定：
                                </th>
                                <td colspan="3">
                                     <textarea id="txtScrapBefore" runat="server" class="input5" rows="3" cols="40"></textarea>
                                    <input type="button" id="btnScrapBefore" style="background-color:#ccc;" value="删除该条设置" />
                                </td>
                            </tr>
                           <tr>
                                <th>
                                   离站后退票规定：
                                </th>
                                <td colspan="3">
                                    <textarea id="txtScrapAfter" runat="server" class="input5" rows="3" cols="40"></textarea>
                                     <input type="button" id="btnScrapAfter" style="background-color:#ccc;" value="删除该条设置" />
                                </td>
                            </tr>
                            </tbody>
                            <tbody id="changeTitle">
                            <tr>
                                <th>
                                    改期规定：
                                </th>
                                <td colspan="3">
                                    <textarea id="txtChange" runat="server" class="input5" rows="3" cols="40"></textarea>
                                     <input type="button" id="btnChange" style="width:120px" class="button" value="离站前后分别设置" />
                                </td>
                            </tr>
                            </tbody>
                            <tbody id="change" style="display:none">
                             <tr>
                                <th>
                                    离站前改期规定：
                                </th>
                                <td colspan="3">
                                     <textarea id="txtChangeBefore" runat="server" class="input5" rows="3" cols="40"></textarea>
                                     <input type="button" id="btnChangeBefore" style="background-color:#ccc;" value="删除该条设置" />
                                </td>
                            </tr>
                             <tr>
                                <th>
                                    离站后改期规定：
                                </th>
                                <td colspan="3">
                                     <textarea id="txtChangeAfter" runat="server" class="input5" rows="3" cols="40"></textarea>
                                     <input type="button" id="btnChangeAfter" style="background-color:#ccc;" value="删除该条设置" />
                                </td>
                            </tr>
                            </tbody>
                            <tr>
                                <th>
                                    签转规定：
                                </th>
                                <td colspan="3">
                                     <textarea id="txtEndorse" runat="server" class="input5" rows="3" cols="40"></textarea>
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="4">
                                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button" 
                                        OnClientClick="return btnSubmit()" onclick="btnSave_Click" />&nbsp;&nbsp;
                                    <input type="button" class="button" id="btnBack" runat="server" value="返回" />
                                </td>
                            </tr>
                    </table>
                </dd>
            </dl>
        </div>
        <br />
        <br />
        <br />
         <div class="title" id="dataList" runat="server">
            <dl>
                <dt>
                 <a href="javascript:void(0);" style="background-color:#ccc; font-size:larger;">以下为已添加的详细信息</a>
                 &nbsp;&nbsp;&nbsp;
                 历史添加信息仅供参考防止您重复添加同舱位避免出错
                   </dt>
                <dd>
                <div class="gundong">
                    <asp:GridView ID="gvRefundChangeTecket" runat="server"
                        AutoGenerateColumns="False" CssClass="list tab3">
                        <Columns>
                            <asp:BoundField DataField="Bunks" HeaderText="舱位" />
                            <asp:TemplateField HeaderText="离站前退票规定">
                                <ItemTemplate>
                                    <div class="epty"><%# Eval("ScrapBefore")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="离站后退票规定">
                                <ItemTemplate>
                                    <div class="epty"><%# Eval("ScrapAfter")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="离站前改期规定">
                                <ItemTemplate>
                                    <div class="epty"><%# Eval("ChangeBefore")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="离站后改期规定">
                                <ItemTemplate>
                                    <div class="epty"><%# Eval("ChangeAfter")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="签转规定">
                                <ItemTemplate>
                                    <div class="epty"><%# Eval("Endorse")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                    </div>
                </dd>
            </dl>
        </div>
    </div>
    <asp:HiddenField ID="hfdScrap" runat="server" />
    <asp:HiddenField ID="hfdChange" runat="server" />
    </form>
</body>
</html>
<script src="../js/jquery.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        if ($("#hfdScrap").val() == "split") {
            $("#scrap").show();
            $("#scrapTitle").hide();
        }
        if ($("#hfdChange").val() == "split") {
            $("#change").show();
            $("#changeTitle").hide();
        }
        $("#btnSubScrap").click(function () {
            $("#scrap").show();
            $("#scrapTitle").hide();
            $("#hfdScrap").val("split");
            $("#txtScrapBefore,#txtScrapAfter").val($("#txtScrap").val());
        });
        $("#btnChange").click(function () {
            $("#change").show();
            $("#changeTitle").hide();
            $("#hfdChange").val("split");
            $("#txtChangeBefore,#txtChangeAfter").val($("#txtChange").val());
        });
        $("#btnScrapBefore").click(function () {
            $("#scrap").hide();
            $("#scrapTitle").show();
            $("#hfdScrap").val("merge");
            $("#txtScrap").val($("#txtScrapAfter").val());
        });
        $("#btnScrapAfter").click(function () {
            $("#scrap").hide();
            $("#scrapTitle").show();
            $("#hfdScrap").val("merge");
            $("#txtScrap").val($("#txtScrapBefore").val());
        });
        $("#btnChangeBefore").click(function () {
            $("#change").hide();
            $("#changeTitle").show();
            $("#hfdChange").val("merge");
            $("#txtChange").val($("#txtChangeAfter").val());
        });
        $("#btnChangeAfter").click(function () {
            $("#change").hide();
            $("#changeTitle").show();
            $("#hfdChange").val("merge");
            $("#txtChange").val($("#txtChangeBefore").val());
        });
    })
    function btnSubmit() {
        var txtBunks = $.trim($("#txtBunks").val());
        if (txtBunks == "") {
            alert("舱位不能为空!");
            $("#txtBunks").select();
            return false;
        }
//        var bunksPattern = /^[a-z1-9A-Z\/]{1,}$/;
//        if (!bunksPattern.test(txtBunks)) {
//            alert("舱位格式错误!");
//            $("#txtBunks").select();
//            return false;
//        }
        var scrap = $("#hfdScrap").val();
        var change = $("#hfdChange").val();
        if (scrap == "split") {
            if ($.trim($("#txtScrapBefore").val()).length > 2000) {
                alert("离站前废票规定格式不正确，最多2000个字符");
                $("#txtScrapBefore").select();
                return false;
            }
            if ($.trim($("#txtScrapAfter").val()).length > 2000) {
                alert("离站后废票规定格式不正确，最多2000个字符");
                $("#txtScrapAfter").select();
                return false;
            }
        } else {
            if ($.trim($("#txtScrap").val()).length > 2000) {
                alert("废票规定格式不正确，最多2000个字符");
                $("#txtScrap").select();
                return false;
            }
        }

        if (change == "split") {
            if ($.trim($("#txtChangeBefore").val()).length > 2000) {
                alert("离站前改期规定格式不正确，最多2000个字符");
                $("#txtChangeBefore").select();
                return false;
            }
            if ($.trim($("#txtChangeAfter").val()).length > 2000) {
                alert("离站后改期规定格式不正确，最多2000个字符");
                $("#txtChangeAfter").select();
                return false;
            }
        } else {
            if ($.trim($("#txtChange").val()).length > 2000) {
                alert("改期规定格式不正确，最多2000个字符");
                $("#txtChange").select();
                return false;
            }
        }
        if ($.trim($("#txtEndorse").val()).length > 2000) {
            alert("签转规定格式不正确，最多2000个字符");
            $("#txtEndorse").select();
            return false;
        }
    }
</script>