<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dictionary.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.SystemManagement.Dictionary"  EnableSessionState="ReadOnly"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>字典表设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>系统管理</span>»<span>字典表设置</span>
        </div>
        <div class="title">
            <div class="zdType">
                <asp:DataList ID="DirctionaryCate" runat="server">
                <ItemTemplate>
                        <a href='Dictionary.aspx?categoryId=<%# Eval("TypeValue") %>'><%# Eval("TypeName")%></a>
                </ItemTemplate>
                </asp:DataList>
            </div>
            <div class="zdContents">
                <div id="divClass">
                    <div id="divInfo" class="breadcrumbs"><img src="../images/icon.png"/>&nbsp;&nbsp;<asp:Label runat="server" ID="lblDictionaryName"></asp:Label></div>
                    <asp:GridView ID="gvSpecialType" runat="server" AutoGenerateColumns="False" 
                        CssClass="tab3 list" onrowcommand="gvSpecialType_RowCommand" EmptyDataText="无满足条件的数据!">
                        <Columns>
                            <asp:TemplateField HeaderText="序号" ItemStyle-Width="30px">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1%> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="名称" DataField="Name"/>
                            <asp:BoundField HeaderText="值" DataField="Value"/>
                           <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <div id="epty"><%#Eval("Remark") %></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-Width="35px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="linkUpdate" runat="server" CommandArgument='<%#Eval("ID") %>' CommandName="update" Text='修改'></asp:LinkButton>
                                    <asp:LinkButton ID="linkDel" runat="server" CommandName="dictionaryDel" CommandArgument='<%#Eval("ID") %>'
                                        Text="删除" OnClientClick='return confirm("您确定要删除吗？")'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                    <asp:Button ID="btnAdd" runat="server" CssClass="button" style="margin-top:10px; margin-left:10px;" Text="添加" onclick="btnAdd_Click" />
                    <asp:Button ID="btnRefresh" runat="server" CssClass="button" Text="刷新缓存" 
                        onclick="btnRefresh_Click" Width="100px" />
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </div>
    <div class="clear"></div>
    <input type="hidden" value="0" runat="server" id="iType" />
    </form>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<style type="text/css"> 
    .one{ background-color:Red;}
    #epty
    {
        width:250px;
        white-space:nowrap;
        text-overflow: ellipsis;
        -o-text-overflow: ellipsis;
        overflow: hidden;
    }
</style>