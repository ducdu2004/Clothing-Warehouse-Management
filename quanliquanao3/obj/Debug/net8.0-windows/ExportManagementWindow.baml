   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   H Dquanliquanao3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:quanliquanao3quanliquanao3 5   	   *   $quanliquanao3.ExportManagementWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   -localclr-namespace:quanliquanao3     5   	     � Title$  ExportManagementWindow��5	   	   $	��450��6(   $	��800��65   .��5
      � $��0,0,0,32q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   $��198.04��6*   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $
��Left=�6.   $��Center=�6I   $��24��6d   $	��737��6p   .+�5      �� $��Ngày Xuất:��6   $��Center=�6+   $��0,0,5,0q�6F   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ dpImportDate��5      $	��150��6/   
 �� Now   SelectedDate# Z 6;   $��0,0,10,0q�6f   �� 5      $��Người mua:��6   $��Center=�6+   $��0,0,5,0q�6F   �� 5      -   5         ��Name$ cbCustomers��5      $	��200��6,    �� DisplayMemberPath$ CustomerName��68    �� SelectedValuePath$ 
CustomerId��6Y   $��0,0,10,0q�6x   6�   5      &   System.Windows.Controls.DataGrid  5   
   -   5        	  Name$	 dgExportDetails��5      $��1��6,   
   AutoGenerateColumns$
 False��69      CanUserAddRows$
 True��6U   $��
0,0,265,10q�6k      Columns 5      4  .System.Windows.Controls.DataGridComboBoxColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Sản phẩm��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 6;      SelectedValuePath$ 	ProductId��5   !      DisplayMemberPath$ ProductName��5    !      SelectedValueBinding 5   !   �� *	ProductId+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   ItemsSource 5!   !   �� *ProductsList+ �� RelativeSource ��  �� AncestorType
 Window0  *System.Windows.Controls.DataGridTextColumn  5#      $ Số lượng��6%   $	 100 6a   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *
Quantity+$ TwoWay=�6a   5%      �� 5)   
   $��1��6   $ 
Horizontal=�6"   $
��Left=�6;   $��540,10,0,10q�6V   $	��250��6k   .+�5*      �� -   5        $��Lưu phiếu xuất��5*      $	��150��6/   $��30��6;   $��5q�6G   6R   �� 5+      -   5        $��Thoát��5+      $��80��6%   $��30��60   $��5q�6<   6G   5,      �� 5-   
   $��2��6   .+�5.        -   5        $	 dgExport��5.   $   $	��200��6   $
 False��66    5/        50      $ 
ImportDate��6)    6=   �� *
ExportDate+6^     51      $ CustomerName��6)    6?   �� *CustomerName+6b     52      $ ProductName��6)    6>   �� *ProductName+6`     53      $ Quantity��6)    6;   �� *
Quantity+6Z     54      $ 
TotalPrice��6)    6=   �� *
TotalPrice+6^   56      57      58      59      