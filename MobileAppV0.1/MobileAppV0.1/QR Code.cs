using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.AppCompat;
using static Android.Gms.Vision.Detector;
using Android.Views;
using Android.Gms.Vision.Barcodes;
using Android.Gms.Vision;
using Android.Graphics;
using Android.Runtime;
using System;
using Android;
using Android.Content.PM;
using Android.Util;
using Android.Content;
//dodanie dyrektyw do obsługi QR Code

namespace MobileAppV0._1
{
    [Activity(Label = "QR_Code")]
    public class QR_Code : Activity, ISurfaceHolderCallback, IProcessor
    {
        private Button BackButton;
        private SurfaceView cameraPreview; //część ekranu w której wyświetli się aparat
        private TextView txtResult;  //rezultat Qr kodu
        private BarcodeDetector barcodeDetector;
        private CameraSource cameraSource; //cameraSource - żródło, tj. aparat z telefonu
        const int RequestCameraPermissionID = 1001; //unikalny ID potrzebny do uzyskania dostępu do kamery -> należy też dodać odpowiednie pozwolenia w AndroidManifest

        #region Odpalenie aparatu
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestCameraPermissionID:
                    {
                        if (grantResults[0] == Permission.Granted)  //sprawdzenie czy można udzielić dostępu do kamery
                        {
                            if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
                            {
                                //Request permission
                                ActivityCompat.RequestPermissions(this, new string[]
                                {
                                     Manifest.Permission.Camera
                                }, RequestCameraPermissionID);
                                return;
                            }
                            try
                            {
                                cameraSource.Start(cameraPreview.Holder);  //uruchomienie kamery
                            }
                            catch (InvalidOperationException e)
                            {
                                txtResult.Text = e.Message;  //obsługa błędu - wyświetlenie go na ekranie
                            }
                        }
                    }
                    break;
            }
        }
#endregion

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.QR_Code);

            cameraPreview = FindViewById<SurfaceView>(Resource.Id.cameraPreview);
            txtResult = FindViewById<TextView>(Resource.Id.txtResult);
            BackButton = FindViewById<Button>(Resource.Id.BackButton);

            barcodeDetector = new BarcodeDetector.Builder(this)
                .SetBarcodeFormats(BarcodeFormat.QrCode)
                .Build();
            cameraSource = new CameraSource
                .Builder(this, barcodeDetector)
                .SetRequestedPreviewSize(640, 480)
                .Build();

            cameraPreview.Holder.AddCallback(this);
            barcodeDetector.SetProcessor(this);
            //ustawienie kontrolek i elementów

            BackButton.Click += delegate  //obsługa powrotu do wyszukiwarki
            {
                Intent intent = new Intent(this, typeof(Wyszukiwarka));
                this.StartActivity(intent);
                Finish();
            };
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {

        }

        public void SurfaceCreated(ISurfaceHolder holder) //utworzenie przestrzeni do obsługi kamery wraz z wystartowaniem
        {
            if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
            {
                //Request permission
                ActivityCompat.RequestPermissions(this, new string[]
                {
                   Manifest.Permission.Camera
                }, RequestCameraPermissionID);
                return;
            }
            try
            {
                cameraSource.Start(cameraPreview.Holder);
            }
            catch (InvalidOperationException err)
            {
                txtResult.Text = err.Message; //obsługa błędu
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)  //wyłączenie kamery, np. zmiana okienka
        {
            cameraSource.Stop();
        }

        public void ReceiveDetections(Detections detections) //wykrywanie QR Code
        {
            SparseArray qrcodes = detections.DetectedItems;
            if (qrcodes.Size() != 0)
            {
                txtResult.Post(() => {
                    txtResult.Text = ((Barcode)qrcodes.ValueAt(0)).RawValue;  //wyświetlenie zawartości QR kodu
                });
            }
        }

        public void Release()
        {

        }
    }
}