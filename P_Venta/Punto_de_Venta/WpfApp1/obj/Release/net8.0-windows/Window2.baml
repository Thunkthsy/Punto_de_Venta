   M S B A M L   `   `   `  ���� B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1      WpfApp1.Window2   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     'localclr-namespace:WpfApp1       � Title$  Buscar Producto��$	��450��$	��800�� � 	Resources     BooleanToVisibilityConverter��%   &	        �� .��� $��10q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* �.+��� -   $��	txtSearch��$��0��$��30��$��16(�$	��Top=�.������  �� 
TargetType" ��.���  �� Property � �� �� Value ��  �� 
TargetType" ��.R�� .+���  ��Name$ PART_ContentHost���� $��Buscar Producto...�� J� IsHitTestVisible$ False��
�������$��5,0,0,0q�$��Center=�y��� *Text.IsEmpty+	 �� RelativeSource	 �� *TemplatedParent+
 �� 	Converter#
 [  .���� -   $��
lblMessage��$��1��$��30��$��14(�
���  ��$y�	Collapsed=�&   System.Windows.Controls.DataGrid  -   $��dGProd_Search��$��2��   AutoGenerateColumns$ False��$��0,10,0,0q����� *	Productos+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Código��   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Codigo+  $ Nombre��$ *  �� *Nombre+  $ Descripción��$ 2*  �� *Descripcion+  $ Precio��$
 Auto  �� *Precio+  $ 
Existencia��$
 Auto  �� *
Existencia+  $ Medida��$
 Auto  �� *Medida+  $ Departamento��$
 Auto  �� *Departamento+4  .System.Windows.Controls.DataGridTemplateColumn  $ Acción��$	 100    CellTemplate �� .R��� -   $��Agregar�� � DataContext �� 