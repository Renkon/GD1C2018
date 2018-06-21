using System.Windows.Forms;
namespace FrbaHotel
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        { 
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iniciarSesiónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarSesiónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reservasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevaReservaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificarReservaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelarReservaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rolesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoRolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificarRolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borrarRolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoUsuarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificarUsuarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borrarUsuarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoClienteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificarClienteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borrarClienteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hotelesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoHotelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificarHotelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarTemporalmenteHotelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.habitacionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevaHabitaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificarHabitaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eliminarHabitaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.estadíaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registrarEstadíaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consumiblesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registrarConsumiblesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facturaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facturarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.estadísticasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generarListadoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripMenuItem,
            this.reservasToolStripMenuItem,
            this.rolesToolStripMenuItem,
            this.usuariosToolStripMenuItem,
            this.clientesToolStripMenuItem,
            this.hotelesToolStripMenuItem,
            this.habitacionesToolStripMenuItem,
            this.estadíaToolStripMenuItem,
            this.consumiblesToolStripMenuItem,
            this.facturaciónToolStripMenuItem,
            this.estadísticasToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1424, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iniciarSesiónToolStripMenuItem,
            this.cerrarSesiónToolStripMenuItem});
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.statusToolStripMenuItem.Text = "Sesión";
            // 
            // iniciarSesiónToolStripMenuItem
            // 
            this.iniciarSesiónToolStripMenuItem.Name = "iniciarSesiónToolStripMenuItem";
            this.iniciarSesiónToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.iniciarSesiónToolStripMenuItem.Text = "Iniciar sesión";
            this.iniciarSesiónToolStripMenuItem.Click += new System.EventHandler(this.iniciarSesiónToolStripMenuItem_Click);
            // 
            // cerrarSesiónToolStripMenuItem
            // 
            this.cerrarSesiónToolStripMenuItem.Name = "cerrarSesiónToolStripMenuItem";
            this.cerrarSesiónToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.cerrarSesiónToolStripMenuItem.Text = "Cerrar sesión";
            this.cerrarSesiónToolStripMenuItem.Click += new System.EventHandler(this.cerrarSesiónToolStripMenuItem_Click);
            // 
            // reservasToolStripMenuItem
            // 
            this.reservasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevaReservaToolStripMenuItem,
            this.modificarReservaToolStripMenuItem,
            this.cancelarReservaToolStripMenuItem});
            this.reservasToolStripMenuItem.Name = "reservasToolStripMenuItem";
            this.reservasToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.reservasToolStripMenuItem.Text = "Reservas";
            // 
            // nuevaReservaToolStripMenuItem
            // 
            this.nuevaReservaToolStripMenuItem.Name = "nuevaReservaToolStripMenuItem";
            this.nuevaReservaToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.nuevaReservaToolStripMenuItem.Text = "Nueva reserva";
            this.nuevaReservaToolStripMenuItem.Click += new System.EventHandler(this.nuevaReservaToolStripMenuItem_Click);
            // 
            // modificarReservaToolStripMenuItem
            // 
            this.modificarReservaToolStripMenuItem.Name = "modificarReservaToolStripMenuItem";
            this.modificarReservaToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.modificarReservaToolStripMenuItem.Text = "Modificar reserva";
            this.modificarReservaToolStripMenuItem.Click += new System.EventHandler(this.modificarReservaToolStripMenuItem_Click);
            // 
            // cancelarReservaToolStripMenuItem
            // 
            this.cancelarReservaToolStripMenuItem.Name = "cancelarReservaToolStripMenuItem";
            this.cancelarReservaToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.cancelarReservaToolStripMenuItem.Text = "Cancelar reserva";
            this.cancelarReservaToolStripMenuItem.Click += new System.EventHandler(this.cancelarReservaToolStripMenuItem_Click);
            // 
            // rolesToolStripMenuItem
            // 
            this.rolesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoRolToolStripMenuItem,
            this.modificarRolToolStripMenuItem,
            this.borrarRolToolStripMenuItem});
            this.rolesToolStripMenuItem.Name = "rolesToolStripMenuItem";
            this.rolesToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.rolesToolStripMenuItem.Text = "Roles";
            // 
            // nuevoRolToolStripMenuItem
            // 
            this.nuevoRolToolStripMenuItem.Name = "nuevoRolToolStripMenuItem";
            this.nuevoRolToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.nuevoRolToolStripMenuItem.Text = "Nuevo rol";
            this.nuevoRolToolStripMenuItem.Click += new System.EventHandler(this.nuevoRolToolStripMenuItem_Click);
            // 
            // modificarRolToolStripMenuItem
            // 
            this.modificarRolToolStripMenuItem.Name = "modificarRolToolStripMenuItem";
            this.modificarRolToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.modificarRolToolStripMenuItem.Text = "Modificar rol";
            this.modificarRolToolStripMenuItem.Click += new System.EventHandler(this.modificarRolToolStripMenuItem_Click);
            // 
            // borrarRolToolStripMenuItem
            // 
            this.borrarRolToolStripMenuItem.Name = "borrarRolToolStripMenuItem";
            this.borrarRolToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.borrarRolToolStripMenuItem.Text = "Borrar rol";
            this.borrarRolToolStripMenuItem.Click += new System.EventHandler(this.borrarRolToolStripMenuItem_Click);
            // 
            // usuariosToolStripMenuItem
            // 
            this.usuariosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoUsuarioToolStripMenuItem,
            this.modificarUsuarioToolStripMenuItem,
            this.borrarUsuarioToolStripMenuItem});
            this.usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
            this.usuariosToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.usuariosToolStripMenuItem.Text = "Usuarios";
            // 
            // nuevoUsuarioToolStripMenuItem
            // 
            this.nuevoUsuarioToolStripMenuItem.Name = "nuevoUsuarioToolStripMenuItem";
            this.nuevoUsuarioToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.nuevoUsuarioToolStripMenuItem.Text = "Nuevo usuario";
            this.nuevoUsuarioToolStripMenuItem.Click += new System.EventHandler(this.nuevoUsuarioToolStripMenuItem_Click);
            // 
            // modificarUsuarioToolStripMenuItem
            // 
            this.modificarUsuarioToolStripMenuItem.Name = "modificarUsuarioToolStripMenuItem";
            this.modificarUsuarioToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.modificarUsuarioToolStripMenuItem.Text = "Modificar usuario";
            this.modificarUsuarioToolStripMenuItem.Click += new System.EventHandler(this.modificarUsuarioToolStripMenuItem_Click);
            // 
            // borrarUsuarioToolStripMenuItem
            // 
            this.borrarUsuarioToolStripMenuItem.Name = "borrarUsuarioToolStripMenuItem";
            this.borrarUsuarioToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.borrarUsuarioToolStripMenuItem.Text = "Borrar usuario";
            this.borrarUsuarioToolStripMenuItem.Click += new System.EventHandler(this.borrarUsuarioToolStripMenuItem_Click);
            // 
            // clientesToolStripMenuItem
            // 
            this.clientesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoClienteToolStripMenuItem,
            this.modificarClienteToolStripMenuItem,
            this.borrarClienteToolStripMenuItem});
            this.clientesToolStripMenuItem.Name = "clientesToolStripMenuItem";
            this.clientesToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.clientesToolStripMenuItem.Text = "Clientes";
            // 
            // nuevoClienteToolStripMenuItem
            // 
            this.nuevoClienteToolStripMenuItem.Name = "nuevoClienteToolStripMenuItem";
            this.nuevoClienteToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.nuevoClienteToolStripMenuItem.Text = "Nuevo cliente";
            this.nuevoClienteToolStripMenuItem.Click += new System.EventHandler(this.nuevoClienteToolStripMenuItem_Click);
            // 
            // modificarClienteToolStripMenuItem
            // 
            this.modificarClienteToolStripMenuItem.Name = "modificarClienteToolStripMenuItem";
            this.modificarClienteToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.modificarClienteToolStripMenuItem.Text = "Modificar cliente";
            this.modificarClienteToolStripMenuItem.Click += new System.EventHandler(this.modificarClienteToolStripMenuItem_Click);
            // 
            // borrarClienteToolStripMenuItem
            // 
            this.borrarClienteToolStripMenuItem.Name = "borrarClienteToolStripMenuItem";
            this.borrarClienteToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.borrarClienteToolStripMenuItem.Text = "Borrar cliente";
            this.borrarClienteToolStripMenuItem.Click += new System.EventHandler(this.borrarClienteToolStripMenuItem_Click);
            // 
            // hotelesToolStripMenuItem
            // 
            this.hotelesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoHotelToolStripMenuItem,
            this.modificarHotelToolStripMenuItem,
            this.cerrarTemporalmenteHotelToolStripMenuItem});
            this.hotelesToolStripMenuItem.Name = "hotelesToolStripMenuItem";
            this.hotelesToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.hotelesToolStripMenuItem.Text = "Hoteles";
            // 
            // nuevoHotelToolStripMenuItem
            // 
            this.nuevoHotelToolStripMenuItem.Name = "nuevoHotelToolStripMenuItem";
            this.nuevoHotelToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.nuevoHotelToolStripMenuItem.Text = "Nuevo hotel";
            this.nuevoHotelToolStripMenuItem.Click += new System.EventHandler(this.nuevoHotelToolStripMenuItem_Click);
            // 
            // modificarHotelToolStripMenuItem
            // 
            this.modificarHotelToolStripMenuItem.Name = "modificarHotelToolStripMenuItem";
            this.modificarHotelToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.modificarHotelToolStripMenuItem.Text = "Modificar hotel";
            this.modificarHotelToolStripMenuItem.Click += new System.EventHandler(this.modificarHotelToolStripMenuItem_Click);
            // 
            // cerrarTemporalmenteHotelToolStripMenuItem
            // 
            this.cerrarTemporalmenteHotelToolStripMenuItem.Name = "cerrarTemporalmenteHotelToolStripMenuItem";
            this.cerrarTemporalmenteHotelToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.cerrarTemporalmenteHotelToolStripMenuItem.Text = "Cerrar temporalmente hotel";
            this.cerrarTemporalmenteHotelToolStripMenuItem.Click += new System.EventHandler(this.cerrarTemporalmenteHotelToolStripMenuItem_Click);
            // 
            // habitacionesToolStripMenuItem
            // 
            this.habitacionesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevaHabitaciónToolStripMenuItem,
            this.modificarHabitaciónToolStripMenuItem,
            this.eliminarHabitaciónToolStripMenuItem});
            this.habitacionesToolStripMenuItem.Name = "habitacionesToolStripMenuItem";
            this.habitacionesToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.habitacionesToolStripMenuItem.Text = "Habitaciones";
            // 
            // nuevaHabitaciónToolStripMenuItem
            // 
            this.nuevaHabitaciónToolStripMenuItem.Name = "nuevaHabitaciónToolStripMenuItem";
            this.nuevaHabitaciónToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.nuevaHabitaciónToolStripMenuItem.Text = "Nueva habitación";
            this.nuevaHabitaciónToolStripMenuItem.Click += new System.EventHandler(this.nuevaHabitaciónToolStripMenuItem_Click);
            // 
            // modificarHabitaciónToolStripMenuItem
            // 
            this.modificarHabitaciónToolStripMenuItem.Name = "modificarHabitaciónToolStripMenuItem";
            this.modificarHabitaciónToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.modificarHabitaciónToolStripMenuItem.Text = "Modificar habitación";
            this.modificarHabitaciónToolStripMenuItem.Click += new System.EventHandler(this.modificarHabitaciónToolStripMenuItem_Click);
            // 
            // eliminarHabitaciónToolStripMenuItem
            // 
            this.eliminarHabitaciónToolStripMenuItem.Name = "eliminarHabitaciónToolStripMenuItem";
            this.eliminarHabitaciónToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.eliminarHabitaciónToolStripMenuItem.Text = "Cerrar temporalmente habitación";
            this.eliminarHabitaciónToolStripMenuItem.Click += new System.EventHandler(this.eliminarHabitaciónToolStripMenuItem_Click);
            // 
            // estadíaToolStripMenuItem
            // 
            this.estadíaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registrarEstadíaToolStripMenuItem});
            this.estadíaToolStripMenuItem.Name = "estadíaToolStripMenuItem";
            this.estadíaToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.estadíaToolStripMenuItem.Text = "Estadía";
            // 
            // registrarEstadíaToolStripMenuItem
            // 
            this.registrarEstadíaToolStripMenuItem.Name = "registrarEstadíaToolStripMenuItem";
            this.registrarEstadíaToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.registrarEstadíaToolStripMenuItem.Text = "Registrar estadía";
            // 
            // consumiblesToolStripMenuItem
            // 
            this.consumiblesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registrarConsumiblesToolStripMenuItem});
            this.consumiblesToolStripMenuItem.Name = "consumiblesToolStripMenuItem";
            this.consumiblesToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.consumiblesToolStripMenuItem.Text = "Consumibles";
            // 
            // registrarConsumiblesToolStripMenuItem
            // 
            this.registrarConsumiblesToolStripMenuItem.Name = "registrarConsumiblesToolStripMenuItem";
            this.registrarConsumiblesToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.registrarConsumiblesToolStripMenuItem.Text = "Registrar consumibles";
            // 
            // facturaciónToolStripMenuItem
            // 
            this.facturaciónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.facturarToolStripMenuItem});
            this.facturaciónToolStripMenuItem.Name = "facturaciónToolStripMenuItem";
            this.facturaciónToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.facturaciónToolStripMenuItem.Text = "Facturación";
            // 
            // facturarToolStripMenuItem
            // 
            this.facturarToolStripMenuItem.Name = "facturarToolStripMenuItem";
            this.facturarToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.facturarToolStripMenuItem.Text = "Facturar";
            // 
            // estadísticasToolStripMenuItem
            // 
            this.estadísticasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generarListadoToolStripMenuItem});
            this.estadísticasToolStripMenuItem.Name = "estadísticasToolStripMenuItem";
            this.estadísticasToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.estadísticasToolStripMenuItem.Text = "Estadísticas";
            // 
            // generarListadoToolStripMenuItem
            // 
            this.generarListadoToolStripMenuItem.Name = "generarListadoToolStripMenuItem";
            this.generarListadoToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.generarListadoToolStripMenuItem.Text = "Generar listado";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1424, 756);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "FRBAHotel - EL MONSTRUO DEL LAGO MASER";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem statusToolStripMenuItem;
        private ToolStripMenuItem reservasToolStripMenuItem;
        private ToolStripMenuItem nuevaReservaToolStripMenuItem;
        private ToolStripMenuItem modificarReservaToolStripMenuItem;
        private ToolStripMenuItem cancelarReservaToolStripMenuItem;
        private ToolStripMenuItem rolesToolStripMenuItem;
        private ToolStripMenuItem nuevoRolToolStripMenuItem;
        private ToolStripMenuItem modificarRolToolStripMenuItem;
        private ToolStripMenuItem borrarRolToolStripMenuItem;
        private ToolStripMenuItem usuariosToolStripMenuItem;
        private ToolStripMenuItem nuevoUsuarioToolStripMenuItem;
        private ToolStripMenuItem modificarUsuarioToolStripMenuItem;
        private ToolStripMenuItem borrarUsuarioToolStripMenuItem;
        private ToolStripMenuItem clientesToolStripMenuItem;
        private ToolStripMenuItem nuevoClienteToolStripMenuItem;
        private ToolStripMenuItem modificarClienteToolStripMenuItem;
        private ToolStripMenuItem borrarClienteToolStripMenuItem;
        private ToolStripMenuItem hotelesToolStripMenuItem;
        private ToolStripMenuItem nuevoHotelToolStripMenuItem;
        private ToolStripMenuItem modificarHotelToolStripMenuItem;
        private ToolStripMenuItem cerrarTemporalmenteHotelToolStripMenuItem;
        private ToolStripMenuItem habitacionesToolStripMenuItem;
        private ToolStripMenuItem nuevaHabitaciónToolStripMenuItem;
        private ToolStripMenuItem modificarHabitaciónToolStripMenuItem;
        private ToolStripMenuItem eliminarHabitaciónToolStripMenuItem;
        private ToolStripMenuItem estadíaToolStripMenuItem;
        private ToolStripMenuItem registrarEstadíaToolStripMenuItem;
        private ToolStripMenuItem consumiblesToolStripMenuItem;
        private ToolStripMenuItem registrarConsumiblesToolStripMenuItem;
        private ToolStripMenuItem facturaciónToolStripMenuItem;
        private ToolStripMenuItem facturarToolStripMenuItem;
        private ToolStripMenuItem estadísticasToolStripMenuItem;
        private ToolStripMenuItem generarListadoToolStripMenuItem;
        private ToolStripMenuItem iniciarSesiónToolStripMenuItem;
        private ToolStripMenuItem cerrarSesiónToolStripMenuItem;
    }
}

