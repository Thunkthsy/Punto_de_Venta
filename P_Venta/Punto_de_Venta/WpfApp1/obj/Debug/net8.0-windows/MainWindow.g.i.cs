// Updated by XamlIntelliSenseFileGenerator 28/10/2024 10:35:56 a. m.
#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6F8584E93CAF91968386EE70472FB514E92799DD"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WpfApp1;


namespace WpfApp1
{


    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector
    {


#line 86 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnVentas;

#line default
#line hidden


#line 87 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnInventario;

#line default
#line hidden


#line 88 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnConfiguracion;

#line default
#line hidden


#line 89 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCorte;

#line default
#line hidden


#line 92 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgLogo;

#line default
#line hidden


#line 95 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblCode;

#line default
#line hidden


#line 96 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCode;

#line default
#line hidden


#line 99 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;

#line default
#line hidden


#line 100 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEnter;

#line default
#line hidden


#line 103 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dGProductos;

#line default
#line hidden


#line 136 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblProductos;

#line default
#line hidden


#line 139 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblTotal;

#line default
#line hidden


#line 140 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Cobrar;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.8.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApp1;component/mainwindow.xaml", System.UriKind.Relative);

#line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.btnVentas = ((System.Windows.Controls.Button)(target));

#line 86 "..\..\..\MainWindow.xaml"
                    this.btnVentas.Click += new System.Windows.RoutedEventHandler(this.btnVentas_Click);

#line default
#line hidden
                    return;
                case 2:
                    this.btnInventario = ((System.Windows.Controls.Button)(target));

#line 87 "..\..\..\MainWindow.xaml"
                    this.btnInventario.Click += new System.Windows.RoutedEventHandler(this.btnInventario_Click);

#line default
#line hidden
                    return;
                case 3:
                    this.btnConfiguracion = ((System.Windows.Controls.Button)(target));

#line 88 "..\..\..\MainWindow.xaml"
                    this.btnConfiguracion.Click += new System.Windows.RoutedEventHandler(this.btnConfiguracion_Click);

#line default
#line hidden
                    return;
                case 4:
                    this.btnCorte = ((System.Windows.Controls.Button)(target));

#line 89 "..\..\..\MainWindow.xaml"
                    this.btnCorte.Click += new System.Windows.RoutedEventHandler(this.btnCorte_Click);

#line default
#line hidden
                    return;
                case 5:
                    this.imgLogo = ((System.Windows.Controls.Image)(target));
                    return;
                case 6:
                    this.lblCode = ((System.Windows.Controls.Label)(target));
                    return;
                case 7:
                    this.txtCode = ((System.Windows.Controls.TextBox)(target));

#line 96 "..\..\..\MainWindow.xaml"
                    this.txtCode.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtCode_KeyDown);

#line default
#line hidden

#line 96 "..\..\..\MainWindow.xaml"
                    this.txtCode.GotFocus += new System.Windows.RoutedEventHandler(this.RemoveText);

#line default
#line hidden

#line 96 "..\..\..\MainWindow.xaml"
                    this.txtCode.LostFocus += new System.Windows.RoutedEventHandler(this.AddText);

#line default
#line hidden
                    return;
                case 8:
                    this.btnSearch = ((System.Windows.Controls.Button)(target));

#line 99 "..\..\..\MainWindow.xaml"
                    this.btnSearch.Click += new System.Windows.RoutedEventHandler(this.btnSearch_Click);

#line default
#line hidden
                    return;
                case 9:
                    this.btnEnter = ((System.Windows.Controls.Button)(target));

#line 100 "..\..\..\MainWindow.xaml"
                    this.btnEnter.Click += new System.Windows.RoutedEventHandler(this.btnEnter_Click);

#line default
#line hidden
                    return;
                case 10:
                    this.dGProductos = ((System.Windows.Controls.DataGrid)(target));

#line 104 "..\..\..\MainWindow.xaml"
                    this.dGProductos.CellEditEnding += new System.EventHandler<System.Windows.Controls.DataGridCellEditEndingEventArgs>(this.dGProductos_CellEditEnding);

#line default
#line hidden
                    return;
                case 12:
                    this.lblProductos = ((System.Windows.Controls.Label)(target));
                    return;
                case 13:
                    this.lblTotal = ((System.Windows.Controls.Label)(target));
                    return;
                case 14:
                    this.btn_Cobrar = ((System.Windows.Controls.Button)(target));

#line 140 "..\..\..\MainWindow.xaml"
                    this.btn_Cobrar.Click += new System.Windows.RoutedEventHandler(this.btn_Cobrar_Click);

#line default
#line hidden
                    return;
            }
            this._contentLoaded = true;
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 11:

#line 128 "..\..\..\MainWindow.xaml"
                    ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Quitar_Click);

#line default
#line hidden
                    break;
            }
        }

        internal System.Windows.Controls.ComboBox CboxTicket;
    }
}

