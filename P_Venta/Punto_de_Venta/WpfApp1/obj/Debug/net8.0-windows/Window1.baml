   M S B A M L   `   `   `  ����  WpfApp1    WpfApp1.Window1   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  Tickets Cerrados��5   	   $	��450��6"   $	��800��6/   .��5	      � .+�5   
   &   System.Windows.Controls.Calendar  -   5          Name$ calendar��5      $
��Left=�6%   $��	25,10,0,0q�6@   $	��Top=�6S   �� 5   
   -   5         ��Name$ Btn_BuscarTickets��5      $"��Buscar en fecha seleccionada��6,   $
��Left=�6S   $��
25,183,0,0q�6n   $	��Top=�6�   $	��179��6�   6�   �� 5   
   -   5         ��Name$ listBoxFolios��5      $
��Left=�6)   $��
25,208,0,0q�6D   $	��Top=�6X   $	��179��6p   $	��201��6|   ��5      �� *ClosedFolios+5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dataGridProducts��5      $��219,13,25,25q�6T      AutoGenerateColumns$
 True��5         
IsReadOnly$
 True��5         CanUserAddRows$ False��5         CanUserDeleteRows$ False��5      	   CanUserSortColumns$	 False��5      
 J� IsHitTestVisible$
 False��5      ��5   -   �� *TicketProducts+5      5      5      