   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	   A =Models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:Models;assembly=ModelsModels 5   	       WpfApp1.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   'localclr-namespace:WpfApp1     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7models$clr-namespace:Models;assembly=Models     5   	     � Title$  Menú Principal��5   	   $	��600��5	   	   $
��1200��5
   	    � 
ResizeMode$ NoResize=�5   	      WindowBackgroundBrush#���   5   	    � 	Resources 5       �� Color  ControlBackgroundBrush  ControlForegroundBrush  AccentBrush �� 
TargetType �� Property �� Value �� Triggers P� Property	 P� Value&   System.Windows.Controls.DataGrid=  7System.Windows.Controls.Primitives.DataGridColumnHeader��%�  &	        &	 #     &	 D     &	 e     '�� �     7  7  7  '�� &    7  7  7  '�� �    7  '  6    7  7  7  7  '  O    7  7  7  �� 5   
   $ 	#FFD3D1C5��68   �� 5   
   $ #FFFFFF��69   �� 5   
   $ #333333��69   �� 5   
   $ 	#FFA3CCDE��6.   �� 5   
   " ��6   .�5      ��  � ��8   6+   �� 5       � ��8  6+   �� 5       � ��8  6,   �� 5       � ��$ 14(�6)   �� 5        � ��$	 5,2q�6(    5#      P� 5$       �@J�IsMouseOver$
	 True��61   . �5%      ��  � �� �#E0E0E063   5'      P� 5(       � ��$
	 True��6/   . �5)      ��  � �� �#CCCCCC63   5+      5-      �� 50   
   " ��6   .�51      ��  � ��8   6+   �� 52       � ��8  6+   �� 53       � ��8  6,   �� 54       � ��$ 14(�6)   55      �� 58   
   " ��6   .�59      ��  � ��8   6+   �� 5:       � ��$ 16(�6)   5;      �� 5>   
   "  6   .�5?      ��  � ��8   6+   �� 5@       � ��8  6+   �� 5A       � ��8  6,   �� 5B       � ��$ 14(�6)   �� 5C       �@ RowBackground8  6.   �� 5D        �@ AlternatingRowBackground �#E8E8E869   5F      �� 5I   
   "  6   .�5J      ��  � ��8   6+   �� 5K       � ��8  6+   �� 5L       � ��8  6,   �� 5M       � ��$ 14(�6)   5N      5P      .��5R      � 
 � ColumnDefinitions
 5S   
   �� 5T      $	��49* �6   �� 5U      $
��451* �6   .+�5Y   
   �� -   5         ��Name$ 	BtnVentas��5Y      $��VENTAS��6$   $
��Left=�65   $��	50,41,0,0q�6P   $	��Top=�6c   $	��110��6{   $��30��6�   $��2��6�   �� 5Z   
   -   5        $��btnInventario��5Z      $��
INVENTARIO��6&   $
��Left=�6;   $��
152,41,0,0q�6V   $	��Top=�6j   $	��110��6�   $��30��6�   $��1��6�   �� 5]   
   -   5        $��imgLogo��5]      $��
900,41,0,0q�6   $��/punto de venta.jpg��63   $	��121��6P   $	��120��6\   $	��Top=�6i   $
��Left=�6�   $��1��6�   �� 5`   
   -   5        $��lblCode��5`      $��Código del Producto:��6   $
��Left=�6>   $��	50,86,0,0q�6Y   $	��Top=�6l   $	��164��6�   $��30��6�   $��2��6�   �� 5a   
   -   5        $��txtCode��5a      $0��*Introduce el código del Producto o Nombre��6!   $
��Left=�6R   $��
267,86,0,0q�6m    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��400��6�   $��30��6�    �� TextAlignment$ Center=�6�   $��1��6   �� 5d   
   -   5        $��	btnSearch��5d      $��BUSCAR��6"   $
��Left=�63   $��
152,86,0,0q�6N   $	��Top=�6b   $	��110��6z   $��30��6�   $��1��6�   �� 5e   
   -   5        $��btnEnter��5e      $ ��ENTER -- Ingresar Producto��6!   $
��Left=�6F   $��
685,86,0,0q�6a   $	��Top=�6u   $	��196��6�   $��30��6�   $��1��6�     5h   
   -   5        $��dGProductos��5h      $��50,190,50,89q�6&      AutoGenerateColumns$ False��6<   $��2��5i   Y      CanUserAddRows$ False��5j         CanUserDeleteRows$ False��6"      
IsReadOnly$
 True��6<   $	��320��6N   ��5i      �� *	Productos+   Columns 5k      0  *System.Windows.Controls.DataGridTextColumn  5m      ,  &System.Windows.Controls.DataGridColumn   Header$ Código��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6P      
IsReadOnly$
 True��6Z   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *Codigo+6Z     5p      $ Nombre��6%   $ * 6P   $
 True��6Z    65   �� *Nombre+6Z     5s      $ Descripción��6%   $ * 6Z   $
 True��6d    6:   �� *Descripcion+6d     5v      $ Precio��6%   $ * 6`   $
 True��6j    65   �� *Precio+ �� StringFormat$ C��6j     5y      $ Stock��6%   $ * 6S   $
 True��6]    64   �� *
Existencia+6]     5|      $ Medida��6%   $ * 6P   $
 True��6Z    65   �� *Medida+6Z     5      $ 
Existencia��6%   $ * 6X   $
 True��6b    69   �� *
Existencia+6b     5�      $ Departamento��6%   $ * 6\   $
 True��6f    6;   �� *Departamento+6f     5�      $ Cantidad��6%   $ * 6T   $
 True��6^    67   �� *
Cantidad+6^   4  .System.Windows.Controls.DataGridTemplateColumn  5�      $ Acción��6)   $	 100 69      CellTemplate 5�      �� 5�      .R�5�      �� -	   5        $��Quitar��5�   %    � DataContext 6K   �� 5�      5�      5�      �� 5�   
   -
   5        $��lblProductos��5�      $�� ��6$   $
��Left=�6/   $��
49,520,0,0q�6J   $	��Top=�6^   $	��459��6v   $��30��6�   $��2��6�   �� 5�   
   -   5        $��lblTotal��5�      $��Total: 0.00��6    $
��Left=�66   $��889,511,0,0q�6Q   $	��Top=�6f   $	��138��6~   $��30��6�    J� RenderTransformOrigin$ 
0.45,0.167&�6�   $��1��6�   �� 5�   
   -   5        $ 
btn_Cobrar��5�      $��COBRAR��6%   $
��Left=�66   $��783,511,0,0q�6Q   $	��Top=�6f   $	��110��6~   $��30��6�   $ 0.905,0.475&�6�   $��1��6�   �� 5�   
   -   5         ��Name$ 
CBoxTicket��5�      $
��Left=�5�      $��
50,163,0,0q�5�      $	��Top=�5�      $	��141��5�      $��2��5�       =� ErrorTemplate 5�      Z� ��5�      �� *TicketFolios+ �� SelectedItem 5�      �� *Selected_Folio+ �� Mode$ TwoWay=��� 5�   
   -   5         ��Name$ LblTicketFolio��5�      $
��Left=�5�      $	��Top=�5�      $��267,152,0,0q�5�      $��1��5�   L   ��6   �� *Selected_Folio+$ Ticket: {0}��6L   �� 5�   
   -   5        $ Btn_Pendiente��5�      $��1��6(   $��	PENDIENTE��68   $
��Left=�6L   $��152,155,0,0q�6g   $	��Top=�6|   $��30��6�   $	��110��6�   �� 5�   
   $��Seleccionar Ticket��6   $
��Left=�6-   $��
53,132,0,0q�6H   $	��Top=�6\   $��2��6t   5�      5�      