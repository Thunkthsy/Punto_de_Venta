   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.Window3   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  
Inventario��5   	   $	��450��6   $	��800��6)   .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dGProd_Stock��5         AutoGenerateColumns$ False��5      $��0,60,0,0q�5	         
IsReadOnly$
 True��5         CanUserAddRows$ False��5         CanUserDeleteRows$ False��5      ��5
      �� *	Productos+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Código��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6P   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 65   �� *Codigo+6P     5      $ Nombre��6%   $ * 6P   	 65   �� *Nombre+6P     5      $ Descripción��6%   $ 2* 6Z   	 6:   �� *Descripcion+6Z     5      $ Precio��6%   $
 Auto 6P   	 65   �� *Precio+6P     5      $ Medida��6%   $
 Auto 6P   	 65   �� *Medida+6P     5      $ Departamento��6%   $
 Auto 6\   	 6;   �� *Departamento+6\     5      $ Stock��6%   $
 Auto 6S   	 64   �� *
Existencia+6S   5      �� 5   
   -   5        
 ��Name$
 btn_check_stock��5      $ ��Buscar Productos sin Stock��6*   $
��Left=�6O   $	��Top=�6j   $��10q�6�   6�   �� 5   
   -   5         ��Name$ lb_stock��5      $�� ��6"   $
��Left=�6-   $	��Top=�6H   $��	170,7,0,0q�6`   �� 5   
   -   5        $
 BtnAll��5      $��Buscar Todos��6!   $
��Left=�68   $��	10,35,0,0q�6S   $	��Top=�6f   $	��145��6~   6�   5      5      