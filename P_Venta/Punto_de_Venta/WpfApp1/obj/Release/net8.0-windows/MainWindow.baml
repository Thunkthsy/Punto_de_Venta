   M S B A M L   `   `   `  ���� B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  A =Models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:Models;assembly=ModelsModels     WpfApp1.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     'localclr-namespace:WpfApp1     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     7models$clr-namespace:Models;assembly=Models       � Title$  Menú Principal��$	��600��$
��1200�� � 
ResizeMode$ NoResize=�   WindowBackgroundBrush#���    � 	Resources  �� Color  ControlBackgroundBrush  ControlForegroundBrush  AccentBrush �� 
TargetType �� Property �� Value �� Triggers P� Property	 P� Value&   System.Windows.Controls.DataGrid=  7System.Windows.Controls.Primitives.DataGridColumnHeader��%e  &	        &	      &	 (     &	 ;     '�� P     7  7  7  '�� >    7  7  7  '�� �    7  '  �    7  7  7  7  '  z    7  7  7  �� $ 	#FFD3D1C5���� $ #FFFFFF���� $ #333333���� $ 	#FFA3CCDE���� " ��.���  � ��8   ��  � ��8  ��  � ��8  ��  � ��$ 14(���  � ��$	 5,2q� P�  �@J�IsMouseOver$
	 True��. ���  � �� �#E0E0E0P�  � ��$
	 True��. ���  � �� �#CCCCCC�� " ��.���  � ��8   ��  � ��8  ��  � ��8  ��  � ��$ 14(��� " ��.���  � ��8   ��  � ��$ 16(��� "  .���  � ��8   ��  � ��8  ��  � ��8  ��  � ��$ 14(���  �@ RowBackground8  ��   �@ AlternatingRowBackground �#E8E8E8�� "  .���  � ��8   ��  � ��8  ��  � ��8  ��  � ��$ 14(�.��� 
 � ColumnDefinitions
 �� $	��49* ��� $
��451* �.+��� -    ��Name$ 	BtnVentas��$��VENTAS��$
��Left=�$��	50,41,0,0q�$	��Top=�$	��110��$��30��$��2���� -   $��btnInventario��$��
INVENTARIO��$
��Left=�$��
152,41,0,0q�$	��Top=�$	��110��$��30��$��1���� -   $��imgLogo��$��
900,41,0,0q�$��/punto de venta.jpg��$	��121��$	��120��$	��Top=�$
��Left=�$��1���� -   $��lblCode��$��Código del Producto:��$
��Left=�$��	50,86,0,0q�$	��Top=�$	��164��$��30��$��2���� -   $��txtCode��$0��*Introduce el código del Producto o Nombre��$
��Left=�$��
267,86,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��400��$��30�� �� TextAlignment$ Center=�$��1���� -   $��	btnSearch��$��BUSCAR��$
��Left=�$��
152,86,0,0q�$	��Top=�$	��110��$��30��$��1���� -   $��btnEnter��$ ��ENTER -- Ingresar Producto��$
��Left=�$��
685,86,0,0q�$	��Top=�$	��196��$��30��$��1��  -   $��dGProductos��$��50,190,50,89q�   AutoGenerateColumns$ False��$��2��   CanUserAddRows$ False��   CanUserDeleteRows$ False��   
IsReadOnly$ False��$	��320������ *	Productos+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Código��   Width5  /System.Windows.Controls.DataGridLengthConverter$ *    
IsReadOnly$
 True��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Codigo+  $ Nombre��$ * $
 True�� �� *Nombre+  $ Descripción��$ * $
 True�� �� *Descripcion+  $ Precio��$ * $
 True�� �� *Precio+ �� StringFormat$ C��  $ Stock��$ * $
 True�� �� *
Existencia+  $ Medida��$ * $
 True�� �� *Medida+  $ 
Existencia��$ * $
 True�� �� *
Existencia+  $ Departamento��$ * $
 True�� �� *Departamento+4  .System.Windows.Controls.DataGridTemplateColumn  $ Acción��$	 100    CellTemplate �� .R��� -	   $��Quitar�� � DataContext �� �� -
   $��lblProductos��$�� ��$
��Left=�$��
49,520,0,0q�$	��Top=�$	��459��$��30��$��2���� -   $��lblTotal��$��Total: 0.00��$
��Left=�$��889,511,0,0q�$	��Top=�$	��138��$��30�� J� RenderTransformOrigin$ 
0.45,0.167&�$��1���� -   $ 
btn_Cobrar��$��COBRAR��$
��Left=�$��783,511,0,0q�$	��Top=�$	��110��$��30��$ 0.905,0.475&�$��1���� -    ��Name$ 
CBoxTicket��$
��Left=�$��
50,163,0,0q�$	��Top=�$	��141��$��2�� =� ErrorTemplate Z� ���� *TicketFolios+ �� SelectedItem �� *Selected_Folio+ �� Mode$ TwoWay=� �� TargetNullValue$ DefaultItem���� -     ��Name$  LblTicketFolio��$
��Left=�$	��Top=�$��267,152,0,0q�$��1������ *Selected_Folio+$ Ticket: {0}���� -   $ Btn_Pendiente��$��1��$��	PENDIENTE��$
��Left=�$��152,155,0,0q�$	��Top=�$��30��$	��110���� $��Seleccionar Ticket��$
��Left=�$��
53,132,0,0q�$	��Top=�$��2��