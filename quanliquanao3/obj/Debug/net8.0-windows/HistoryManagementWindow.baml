   M S B A M L   `   `   `  ����H  Dquanliquanao3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:quanliquanao3quanliquanao3  5   	   +    %quanliquanao3.HistoryManagementWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:quanliquanao3     5   	     � Title$  HistoryManagementWindow��5   	   $	��450��6)   $	��800��66   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��1��6!   $��20(�61   $��Black"�6?   $��Center=�6R   $��0,25,0,0q�6o   $
��Wrap=�6�   $'��!Lịch sử quản lý hàng hóa��6�   $��40��6�   $	��340��6�    �� TextAlignment$ Center=�6�   �� 5   
   -   5         ��Name$ cbFilterTime��5       $��1��6   $��10,10,0,139q�66   $	��217��6K   $
��Left=�6W   .@�5      �� $��Tuần��6   �� 5      $��Tháng��6   �� 5      $
��Năm��6   5      �� 5   
   -   5        $ cbTransactionType��5   0   $��1��6   $��1��6#   $��
0,10,0,139q�6K   $	��246��6_   $��Center=�6k   .@�5      �� $��
Tất cả��6   �� 5      $��Nhập��6   �� 5      $��Xuất��6   5      �� 5   
   -   5        $ 	cbProduct��5   0   $��1��6   $��2��6    $��27,10,0,139q�6C   $	��230��6X   $
��Left=�6d    �� DisplayMemberPath$ ProductName��6    �� SelectedValuePath$ 	ProductId��6�   6�   &   System.Windows.Controls.DataGrid  5    
   -   5          Name$ 	dgHistory��5       $��1��6'   $��3��64   $��10,51,10,92q�6H   $��2��6]      AutoGenerateColumns$ False��6n   	   Columns	 5!      0  *System.Windows.Controls.DataGridTextColumn  5"      ,  &System.Windows.Controls.DataGridColumn
   Header$
 	Mã hàng��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6S   1  +System.Windows.Controls.DataGridBoundColumn   Binding 66   �� *
ItemCode+6S     5#      $
 
Tên hàng��6%   $ * 6T    67   �� *
ItemName+6T     5$      $
 Số lượng��6%   $ * 6T    67   �� *
Quantity+6T     5%      $
 Ngày��6%   $ * 6L    63   �� *Date+6L     5&      $
 Loại giao dịch��6%   $ * 6a    6=   �� *TransactionType+6a     5'      $
 Tổng tiền��6%   $ * 6e    6:   �� *Total+ �� StringFormat$ N0��6e   5*      �� 5+   
   $��1��6   $
��Left=�6$   $��140,107,0,0q�6?   $��2��6T   $
��Wrap=�6a   $��Total:��6u   $	��Top=�6�   $��33��6�   $��79��6�   �� 5,   
   -   5         ��Name$ txtTotal��5,      $��1��6%   $��2��65   $
��Left=�6I   $��233,107,0,0q�6d   $��2��6y    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��120��6�   $��33��6�    �� 
IsReadOnly$
 True��6�   50      51      