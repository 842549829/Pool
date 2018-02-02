<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuggestList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.SuggestList"
    EnableEventValidation="false" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户意见反馈</title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link href="/Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .maxWidth {
            max-width: 80px;
            word-break: break-all;
            word-wrap: break-word;   
        }
        td.controls {
            width: 260px;
            padding-left: 30px;
        }
        td.controls div {
            width: 60px;
            float: left;   
        }
        #dataListTable td {
            vertical-align: middle;   
        }.dlLeft
        {
            float:left;
        }
        .warn
        {
            color:Red;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <div class="hd">
        <h3 class="titleBg">
            用户意见反馈</h3>
    </div>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">提交日期：</span>
                            <asp:TextBox id="txtAppliedDateStart" class="text text-s" runat="server"
                                onfocus="WdatePicker({readOnly:true})" />
                            <asp:TextBox id="txtAppliedDateEnd" class="text text-s"  runat="server"
                                onfocus="WdatePicker({readOnly:true,maxDate:'%y-%M-%d'})" />
                            <asp:HiddenField runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                    <td> <div class="input">
                            <span class="name">建议类型：</span>
                        <asp:dropdownlist runat="server" AppendDataBoundItems="True" ID="ddlSuggestType">
                            <asp:ListItem Value="">全部</asp:ListItem>
                        </asp:dropdownlist>
                            </div></td>

                       <td class="btns">
                        <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查&nbsp;&nbsp;&nbsp;询"
                            OnClick="btnQuery_Click" />
                        <input type="button" onclick="ResetSearchOption()" class="btn class2" value="清空条件" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="column table" id='data-list'>
        <asp:Repeater runat="server" ID="datalist" OnPreRender="AddEmptyTemplate" EnableViewState="False">
            <headertemplate>
                <table id="dataListTable">
                <colgroup>
                    <col class="w10" />
                    <col class="w60"/>
                    <col class="w10" span="3" />
                </colgroup>
                <thead>
                <tr>
                    <th>建议类型</th>
                    <th>内容</th>
                    <th>时间</th>
                    <th>建议人</th>
                    <th>联系方式</th>
                </tr>
                </thead>
                <tbody>
            </headertemplate>
            <itemtemplate>
                <tr>
		            <td><%#Eval("SuggestCategory")%></td>
		            <td class="suggestContent"><%#Eval("SuggestContent")%></td>
		            <td><%#Eval("CreateTime")%></td>
		            <td><%#Eval("Employee")%></td>
                    <td><%#Eval("ContractInformation")%></td>
                </tr>
            </itemtemplate>
            <footertemplate>
                </tbody></table>
            </footertemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
        <asp:HiddenField  ID="hfdCompanyId" runat="server"/>
        <asp:HiddenField ID="hfdSearchCondition" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
<script type="text/javascript">
    SaveDefaultData();
    $(function () {
        $(".suggestContent").tipTip({ limitLength: 200, maxWidth: "700px" });
    });
</script>