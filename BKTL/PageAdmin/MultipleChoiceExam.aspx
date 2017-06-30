<%@ Page Title="Đề thi trắc nghiệm" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="MultipleChoiceExam.aspx.cs" Inherits="BKTL.MultipleChoiceExam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-lg-12 title-control-page">
            <h1 class="page-header">Quản lý đề thi trắc nghiệm</h1>
        </div>
    </div>
    <div class="row">
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            ShowMessage("Thông báo lỗi","PhuDX",500);
        });
    </script>
</asp:Content>
