using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


using Plugin.Battery;
using Xamarin.Essentials;


namespace App_FlashEtec
{
    public partial class MainPage : ContentPage
    {

        bool situacao_lanterna = false;

        public MainPage()
        {
            InitializeComponent();

            btn_on_off.Source = ImageSource.FromResource("App_FlashEtec.Imagens.Button_off.jpg");

            informacoes_bateria();
        }

        private async void btn_on_off_Clicked(object sender, EventArgs e)
        {

            try
            {

                if (!situacao_lanterna)
                {

                    situacao_lanterna = true;

                    btn_on_off.Source = ImageSource.FromResource("App_FlashEtec.Imagens.Button_on.jpg");

                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOnAsync();

                }

                else
                {

                    situacao_lanterna = false;

                    btn_on_off.Source = ImageSource.FromResource("App_FlashEtec.Imagens.Button_off.jpg");

                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOffAsync();

                }

            }

            catch(Exception ex)
            {

                await DisplayAlert("Erro", ex.Message, "OK");

            }

        }

        private async void informacoes_bateria()
        {

            try
            {
            
                if(CrossBattery.IsSupported)
                {

                    CrossBattery.Current.BatteryChanged -= Mudanca_Status_Bateria;

                    CrossBattery.Current.BatteryChanged += Mudanca_Status_Bateria;

                }

                else
                {

                    lbl_aviso_bateria.Text = "As informações sobre a Bateria não estão disponíveis.";

                }
            
            }

            catch(Exception ex)
            {

                await DisplayAlert("Erro", ex.Message, "OK");

            }

        }

        private async void Mudanca_Status_Bateria(object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs e)
        {

            try
            {

                // Exibição da Porcentagem da Bateria do Dispositivo

                lbl_carga_restante.Text = e.RemainingChargePercent.ToString() + "%";

                // Situação do Nível de Bateria

                if (e.IsLow)
                {

                    lbl_aviso_bateria.Text = "Atenção! A Bateria está Acabando.";

                }

                else
                {

                    lbl_aviso_bateria.Text = "";

                }

                // Situação da Bateria

                switch (e.Status)
                {

                    case Plugin.Battery.Abstractions.BatteryStatus.Charging:

                        lbl_status_bateria.Text = "Carregando";

                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Discharging:

                        lbl_status_bateria.Text = "Descarregando";

                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Full:

                        lbl_status_bateria.Text = "Carga Completa";

                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.NotCharging:

                        lbl_status_bateria.Text = "Não Carregando";

                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Unknown:

                        lbl_status_bateria.Text = "Desconhecido";

                        break;

                }

                // Fonte de Energia Detectada

                switch (e.PowerSource)
                {

                    case Plugin.Battery.Abstractions.PowerSource.Ac:

                        lbl_fonte_energia.Text = "Carregador";

                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Battery:

                        lbl_fonte_energia.Text = "Bateria Local";

                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Usb:

                        lbl_fonte_energia.Text = "USB";

                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Wireless:

                        lbl_fonte_energia.Text = "Sem Fio";

                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Other:

                        lbl_fonte_energia.Text = "Desconhecida";

                        break;

                }


            }

            catch(Exception ex)
            {

                await DisplayAlert("Erro", ex.Message, "OK");

            }
            
        }

    }
}
