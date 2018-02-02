<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PolicySolutionOfHanging.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.PolicySolutionOfHanging" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品方政策的挂起/解挂</title>
</head>
<body>
    <form id="form1" runat="server">
    <h3 id="tip" runat="server">
        操作提示：</h3> 
    <div class="tips-box radius" id="msg_box" runat="server">
        <i class="icon icon-attention-circle"></i><span runat="server" id="msgTip">灰色部份由平台为您挂起,如需解挂,请您向平台联系</span>
    </div>
    <h3 id="title" class="titleBg" runat="server">
        解挂政策</h3>
    <div class="form">
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w80" />
            </colgroup>
            <tbody>
                <tr>
                    <td class="title">
                        航空公司
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chklist" runat="server" RepeatColumns="10" RepeatDirection="Horizontal"
                            Width="500px" OnDataBound="chklist_DataBound" RepeatLayout="Flow">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="title" id="tdCause" runat="server">
                        解挂原因
                    </td>
                    <td>
                        <asp:TextBox ID="SolutionReason" TextMode="MultiLine" Height="80px" CssClass="text"
                            MaxLength="100" runat="server" Width="80%"></asp:TextBox>
                        <label class="obvious" runat="server" id="lblCause">
                            请输入解挂原因</label>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="btns">
            <asp:Button ID="btnSolution" runat="server" Text="解挂选中政策" CssClass="btn class1" OnClick="btnSolution_Click"
                Visible="false" />
            <asp:Button ID="btnHang" runat="server" Text="挂起选中政策" CssClass="btn class1" OnClick="btnHang_Click"
                Visible="false" />
        </div>
    </div>
    <div class="form" runat="server" id="hung">
        <h3 class="titleBg" >
            挂起政策</h3>
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w80" />
            </colgroup>
            <tbody>
                <tr>
                    <td class="title">
                        航空公司
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chkHung" runat="server" RepeatColumns="10" RepeatDirection="Horizontal"
                            Width="500px" OnDataBound="chklist_DataBound" RepeatLayout="Flow">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        挂起原因
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemark" TextMode="MultiLine" Height="80px" CssClass="text" MaxLength="100"
                            runat="server" Width="80%"></asp:TextBox>
                        <label class="obvious">
                            请输入挂起原因</label>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="btns">
            <asp:Button ID="btnHung" runat="server" Text="挂起选中政策" CssClass="btn class1" 
                onclick="btnHung_Click" />
        </div>
    </div>
    </form>
</body>
</html>
