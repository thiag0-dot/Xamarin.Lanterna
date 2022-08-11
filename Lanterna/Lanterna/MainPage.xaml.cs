using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.Battery;


namespace Lanterna
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Carrega_Informacoes_Bateria();
        }

        bool estado = false;

        
        private async void btnOnOff_Clicked(object sender, EventArgs e)
        {
            estado = !estado;
            try
            {
                if (estado)
                {
                    btnOnOff.Text = "ON";
                    btnOnOff.BorderColor = Color.Green;
                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));
                    await Flashlight.TurnOnAsync();
                }
                else
                {
                    btnOnOff.Text = "OFF";
                    btnOnOff.BorderColor = Color.Red;
                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));
                    await Flashlight.TurnOffAsync();
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Ocorreu um erro: \n", ex.Message, "OK");
            }
        }

        
        private async void Carrega_Informacoes_Bateria()
        {
            try
            {
                if (CrossBattery.IsSupported)
                {
                    CrossBattery.Current.BatteryChanged -= Mudanca_Status_Bateria;
                    CrossBattery.Current.BatteryChanged += Mudanca_Status_Bateria;
                }
                else
                {
                    lbl_bateria_fraca.Text = "As informações sobre bateria estão indisponíveis";
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Ocorreu um erro: \n", ex.Message, "OK");
            }
        }

        private async void Mudanca_Status_Bateria(object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs e)
        {
            try
            {
                lbl_porcentagem_restante.Text = e.RemainingChargePercent.ToString() + "%";

                if (e.IsLow)
                {
                    lbl_bateria_fraca.Text = "Atenção! A Bateria está fraca!";
                }
                else
                {
                    lbl_bateria_fraca.Text = "";
                }

                switch (e.Status)
                {
                    case Plugin.Battery.Abstractions.BatteryStatus.Charging:
                        lbl_status.Text = "Carregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Discharging:
                        lbl_status.Text = "Descarregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Full:
                        lbl_status.Text = "Carregada";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.NotCharging:
                        lbl_status.Text = "Sem carregar";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Unknown:
                        lbl_status.Text = "Desconhecido";
                        break;
                }

                switch (e.PowerSource)
                {
                    case Plugin.Battery.Abstractions.PowerSource.Ac:
                        lbl_fonte_carregamento.Text = "Carregador";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Battery:
                        lbl_fonte_carregamento.Text = "Bateria";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Usb:
                        lbl_fonte_carregamento.Text = "USB";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Wireless:
                        lbl_fonte_carregamento.Text = "Sem fio";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Other:
                        lbl_fonte_carregamento.Text = "Desconhecida";
                        break;
                }
            }
            catch(Exception ex) 
            {
                await DisplayAlert("Ocorreu um erro: \n", ex.Message, "OK");
            }
        }

        
    }
}
