<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="County.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.County"  EnableSessionState="ReadOnly"  enableEventValidation="false"%>
<%@ Register src="../UserControl/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>县城代码维护</title>
</head>
<body>
    <form id="form1" runat="server" >
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>县城代码维护</span>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png"/>&nbsp;查询条件</dt>
                <dd class="searchbk">
                    <table width="97%" border="0" cellpadding="1" cellspacing="1" class="search condition">
                        <tr>
                            <th>
                                代码：
                            </th>
                            <td>
                                <asp:TextBox CssClass="input1" ID="txtCountyCode" runat="server"></asp:TextBox>
                            </td>
                            <th>
                                中文名称：
                            </th>
                            <td>
                             <asp:TextBox CssClass="input1" ID="txtChineseName" runat="server"></asp:TextBox>
                            </td>
                            <th>所属城市名称：</th>
                            <td>
                                <asp:DropDownList ID="ddlCityName" CssClass="input1" runat="server" 
                                    >
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="6">
                                <asp:Button ID="btnSelect" runat="server" Text="查询" CssClass="button" 
                                    OnClientClick="return btnSubmit()" onclick="btnSelect_Click"/>&nbsp;
                                <input type="button" name="button3" value="添加" class="button" onclick="javascript:window.location.href='County_new.aspx?action=add'" />
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png"/>&nbsp;相应列表</dt>
                <dd>
                      <asp:GridView ID="gvCity" runat="server"
                        AutoGenerateColumns="False" EmptyDataText="无满足条件的数据!"
                          CssClass="tab3 list" onrowdeleting="gvCity_RowDeleting"  >
                        <Columns>
                            <asp:BoundField DataField="Code" HeaderText="代码"/>
                            <asp:BoundField DataField="Name" HeaderText="中文名称" />
                            <asp:BoundField DataField="CityCode" HeaderText="所属城市代码"/>
                            <asp:TemplateField HeaderText="所属城市名称">
                                <ItemTemplate>
                                    <asp:Label ID="labProvinceName"  runat="server" Text='<%# Eval("City.Name")%>'  CssClass="fontColor01"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate> 
                                    <a href="County_new.aspx?action=update&code=<%#Eval("Code") %>">修改</a>
                                    <asp:LinkButton ID="linkDel" runat="server" CommandName="Delete" CommandArgument='<%#Eval("Code") %>' OnClientClick='return confirm("确定要删除吗?")'  Text="删除"></asp:LinkButton>                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                </dd>
            </dl>
            <div class="wpager">
                <div class="wpageright"><uc1:Pager ID="Pagerl" runat="server"/></div>
                <div class="clear"></div> 
            </div>
        </div>
    </div>
    <div class="clear"></div>
    </form>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script src="/Scripts/SaveCondition.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    pageName = "County";
    $("#btnSelect").click(function () { SaveSearchCondition(pageName); });
    function btnSubmit() {
        var regu = /^[0-9]*$/;
        var chinese = /[\u4E00-\u9FA5]/;
        var Spelling = /^[a-zA-z]*$/;

        var txtCountyCode = $("#txtCountyCode").val();
        if (txtCountyCode != "" && !($.trim(txtCountyCode).match(regu))) {
            alert("县城代码格式不正确,只能为数字!");
            $("#txtCountyCode").select();
            return false;
        }
        var txtChineseName = $("#txtChineseName").val();
        if (txtChineseName != "" && !($.trim(txtChineseName).match(chinese))) {
            alert("中文名称格式不正确,只能为中文!");
            $("#txtChineseName").select();
            return false;
        }
        var txtSpelling = $("#txtSpelling").val();
        if (txtSpelling != "" && !($.trim(txtSpelling).match(Spelling))) {
            alert("中文全拼格式不正确,只能为字母!");
            $("#txtSpelling").select();
            return false;
        }
        var txtShortSpelling = $("#txtShortSpelling").val();
        if (txtShortSpelling != "" && !($.trim(txtShortSpelling).match(Spelling))) {
            alert("中文简拼格式不正确,只能为字母!");
            $("#txtShortSpelling").select();
            return false;
        }
        var txtHotLevel = $("#txtHotLevel").val();
        if (txtHotLevel != "" && !($.trim(txtHotLevel).match(regu))) {
            alert("热点级别格式不正确,只能为数字!");
            $("#txtHotLevel").select();
            return false;
        }
    }
</script>