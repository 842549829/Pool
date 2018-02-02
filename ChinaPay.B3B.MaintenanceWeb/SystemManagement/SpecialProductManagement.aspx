<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpecialProductManagement.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.SystemManagement.SpecialProductManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特殊产品管理</title>
    <link rel="stylesheet" type="text/css" href="../css/style.css" />
    <style type="text/css"> 
        #epty
        {
            width:180px;
            white-space:nowrap;
            text-overflow: ellipsis;
            -o-text-overflow: ellipsis;
            overflow: hidden;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>系统管理</span>»<span>特殊产品管理</span>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="/images/icon.png" alt=""/>&nbsp;相应列表
                </dt>
                <dd>
                    <asp:GridView ID="gvProductList" runat="server"  EmptyDataText="无满足条件的数据!"
                        AutoGenerateColumns="False"  CssClass="tab3 list" onrowcommand="gvProductList_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="产品ID "/>
                            <asp:BoundField DataField="Name" HeaderText="产品名称" />
                            <asp:TemplateField HeaderText="产品说明">
                                <ItemTemplate>
                                    <div id="epty"><%#Eval("Explain") %></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="描述信息">
                                <ItemTemplate>
                                    <div id="epty"><%#Eval("Describe") %></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="状态">
                                <ItemTemplate>
                                    <div><%#(bool)Eval("Enabled")?"正常":"禁用" %></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <div id="epty"><%#Eval("Remark")%> </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <a href="./SpecialProductManagement_new.aspx?Id=<%#Eval("Id") %>">修改</a>
                                    <asp:LinkButton runat="server" ID="lnkUpdate"  CommandName="Enabled" CommandArgument='<%#Eval("Id") %>'
                                    Text='<%# (bool)Eval("Enabled")?"禁用":"启用" %>'>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                </dd>
            </dl>
        </div>
    </div>
    </form>
</body>
</html>
