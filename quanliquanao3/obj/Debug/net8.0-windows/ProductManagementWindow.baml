   M S B A M L   `   `   `  ����H  Dquanliquanao3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:quanliquanao3quanliquanao3  5   	   +    %quanliquanao3.ProductManagementWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:quanliquanao3     5   	     � Title$  ProductManagementWindow��5   	   $	��450��6)   $	��800��66   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��2��6!   $��Center=�65   $��0,10,0,0q�6R   $
��Wrap=�6d   $��Product Management��6x   $	��Top=�6�   $��24(�6�   $
��Bold"�6�   �� 5   
   $��
10,72,0,79q�6   $��3��6)   .+�5      �� -   5         ��Name$ 
cbCategory��5      $	��200��6+    �� SelectedValuePath$ 
CategoryId��67    �� DisplayMemberPath$ CategoryName��6V   6w   �� 5      -   5        $ cbSize��5      $	��200��6'   64   &   System.Windows.Controls.DataGrid  5      -   5          Name$ 	dgProduct��5      $	��216��6*      AutoGenerateColumns$ False��67      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 
Product ID��6)   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 * 6[   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6=   �� *	ProductId+6[     5      $	 Product Name��6)   $
 * 6_    6?   �� *ProductName+6_     5      $	 Color��6)   $
 * 6R    68   �� *Color+6R     5      $	 Price��6)   $
 * 6R    68   �� *Price+6R     5      $	 Quantity��6)   $
 * 6X    6;   �� *
Quantity+6X   5      5       �� 5!   
   -   5        $��btnAdd��5!      $	��Add��6   $
��Left=�6-   $��	10,73,0,0q�6H   $��2��6[   $	��Top=�6h    J� RenderTransformOrigin$ -0.535,0.564&�6�   $��35��6�   $��60��6�   6�   �� 5"   
   -   5        $��btnEdit��5"      $
��Edit��6    $
��Left=�6/   $��	88,73,0,0q�6J   $��2��6]   $	��Top=�6j   $ -0.535,0.564&�6�   $��35��6�   $��60��6�   6�   �� 5#   
   -   5        $��	btnDelete��5#      $��Delete��6"   $��Center=�63   $��0,73,0,0q�6P   $��2��6b   $	��Top=�6o   $ -0.535,0.564&�6�   $��35��6�   $��60��6�   6�   �� 5$   
   -   5        $��btnReset��5$      $��Reset��6!   $
��Left=�61   $��
247,73,0,0q�6L   $��2��6`   $	��Top=�6m   $ -0.535,0.564&�6�   $��35��6�   $��60��6�   6�   �� 5&   
   -   5         ��Name$ 
inputPanel��5&      $��0��6)   $��1��66   $��10,90,10,38q�6F   $��3��6[   .+�5'      �� $��Product ID: ��6   $��13(�6,   $��Center=�5(      $��	0,0,10,10q�60   $��0��5)      $��0��6%   �� 5*      -	   5        $��txtProductId��5*      $	��200��6*   $	��5,0q�66   $��3q�6C   ${�False��6O   �� 5+      $��ProductName: ��6   $��13(�6-   $��Center=�5,      $��	0,0,10,10q�6   $��0��5-      $��0��6   �� 5.      -
   5        $��txtProductName��5.      $	��200��6,   $	��5,0q�68   $��3q�6E   ${�False��6Q   �� 5/      $��
Category: ��6   $��13(�6*   6:   �� 50      -   5        $��cbCategoryInput��50      $	��200��6.   $	��5,0q�6:   $��3q�6G   ${�False��6S   $ 
CategoryId��6e   $ CategoryName��6�   6�   �� 51      $��Color: ��6   $��13(�6'   67   �� 52      -   5        $��txtColor��52      $	��200��6&   $	��5,0q�62   $��3q�6?   ${�False��6K   �� 53      $��Price: ��6   $��13(�6'   67   �� 54      -   5        $��txtPrice��54      $	��200��6&   $	��5,0q�62   $��3q�6?   ${�False��6K   �� 55      $��
Quantity: ��6   $��13(�6*   6:   �� 56      -   5        $��txtQuantity��56      $	��200��6)   $	��5,0q�65   $��3q�6B   ${�False��6N   �� 57      -   5        $��btnSave��57      $
��Save��6$   $��80��63   $��5q�6>   6I   58      59      5:      