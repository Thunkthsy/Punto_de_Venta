   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.Window2   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  Buscar Producto��5   	   $	��450��6!   $	��800��6.    � 	Resources 5          BooleanToVisibilityConverter��%   &	        �� 5	   
   6'   5
      .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� -   5        $��	txtSearch��5      $��0��6#   $��30��60   $��16(�6<   $	��Top=�5      .��6N   ��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value 5      �� 5       �� 
TargetType" ��6.   .R�5   "   � .+�5   &   ��  ��Name$ PART_ContentHost��63   �� 5    &   $��Buscar Producto...��60    J� IsHitTestVisible$ False��5!   0   
�������$��5,0,0,0q�5#   0   $��Center=�5$   0   y�5%   0   �� *Text.IsEmpty+	 �� RelativeSource	 �� *TemplatedParent+
 �� 	Converter#
 [  5(   #   5)      5+      5,      .��5-      5.      �� 51   
   -   5        $��
lblMessage��51      $��1��6"   $��30��6/   $��14(�6;   
���  ��$y�	Collapsed=�52   !   &   System.Windows.Controls.DataGrid  55   
   -   5        $��dGProd_Search��55      $��2��56         AutoGenerateColumns$ False��57      $��0,10,0,0q�58         
IsReadOnly$
 True��5:         CanUserAddRows$ False��5;         CanUserDeleteRows$ False��5<         SelectionMode$ Single=�5=         SelectionUnit$ FullRow=�5>      ��59      �� *	Productos+   Columns 5@      0  *System.Windows.Controls.DataGridTextColumn  5A      ,  &System.Windows.Controls.DataGridColumn   Header$ Código��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 5C         
IsReadOnly$
 True��5D      1  +System.Windows.Controls.DataGridBoundColumn   Binding 5B      �� *Codigo+5D        5E      $ Nombre��6%   $ * 5G      $
 True��5H       5F      �� *Nombre+5H        5I      $ Descripción��6%   $ 2* 5K      $
 True��5L       5J      �� *Descripcion+5L        5M      $ Precio��6%   $
 Auto 5O      $
 True��5P       5N      �� *Precio+5P        5Q      $ 
Existencia��6%   $
 Auto 5S      $
 True��5T       5R      �� *
Existencia+5T        5U      $ Medida��6%   $
 Auto 5W      $
 True��5X       5V      �� *Medida+5X        5Y      $ Departamento��6%   $
 Auto 5[      $
 True��5\       5Z      �� *Departamento+5\      4  .System.Windows.Controls.DataGridTemplateColumn  5]      $ Acción��6)   $	 100 69      CellTemplate 5^      �� 5_      .R�5`      �� -   5        $��Agregar��5`   %    � DataContext 5b      �� 5c      5e      5g      5h      5i      