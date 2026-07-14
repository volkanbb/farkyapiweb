import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { SiteSettingsService, SiteSetting } from '../../../core/services/site-settings.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent implements OnInit {
  settingsForm: FormGroup;
  isLoading = false;
  isSaving = false;
  successMessage = '';
  errorMessage = '';

  currentSettings: SiteSetting | null = null;

  constructor(
    private fb: FormBuilder,
    private settingsService: SiteSettingsService
  ) {
    this.settingsForm = this.fb.group({
      aboutTitle: [''],
      aboutDescription: [''],
      activityRegionText: [''],
      instagramUrl: [''],
      address: [''],
      phone: [''],
      email: ['']
    });
  }

  ngOnInit(): void {
    this.loadSettings();
  }

  loadSettings(): void {
    this.isLoading = true;
    this.settingsService.getSettings().subscribe({
      next: (settings: SiteSetting) => {
        this.currentSettings = settings || {};
        
        const defaultAboutTitle = "Güvenle İnşa Ediyor, Geleceği Birlikte Yükseltiyoruz";
        const defaultAboutDescription = `<p class="about-text">
  2015 yılından bu yana Bursa'nın İnegöl ilçesinde faaliyet gösteren FARK YAPI, kalite, güven ve müşteri memnuniyetini esas alarak modern yaşam alanları inşa etmektedir.
</p>
<p class="about-text">
  Kurucumuz Faruk Çengel, yaklaşık 40 yıllık müteahhitlik tecrübesine sahip babasından aldığı bilgi, deneyim ve meslek anlayışını kendi vizyonuyla birleştirerek inşaat sektöründeki yolculuğunu sürdürmektedir. Baba mesleğinin getirdiği tecrübeyi çağdaş yapı anlayışıyla buluşturan FARK YAPI, bugüne kadar birçok başarılı projeye imza atmış ve güvenilirliğiyle ön plana çıkmıştır.
</p>
<p class="about-text">
  Her projemizde kaliteli malzeme, sağlam mühendislik, estetik mimari ve zamanında teslim ilkeleriyle hareket ediyor; müşterilerimize yalnızca bir daire değil, güvenli ve huzurlu yaşam alanları sunuyoruz.
</p>
<h3 class="about-subtitle">Vizyonumuz</h3>
<p class="about-text">
  Bursa ve İnegöl başta olmak üzere Türkiye'nin güvenilir ve tercih edilen inşaat firmalarından biri olmak; modern mimariyi, kaliteli işçiliği ve müşteri memnuniyetini bir araya getirerek geleceğe değer katan projeler üretmektir.
</p>

<h3 class="about-subtitle">Misyonumuz</h3>
<p class="about-text">
  Kaliteli malzeme, profesyonel ekip ve dürüst çalışma anlayışıyla güvenli yaşam alanları inşa etmek; müşterilerimizin beklentilerini en üst seviyede karşılayarak, zamanında teslim ve satış sonrası destek hizmetleriyle kalıcı güven oluşturmaktır.
</p>`;

        const defaultActivityRegionText = `<li>FARK YAPI olarak ağırlıklı olarak Bursa / İnegöl bölgesinde faaliyet göstermekte ve bölgenin gelişimine değer katan konut projeleri üretmekteyiz.</li>
<li>Devam Eden Projeler</li>
<li>Devam eden projelerimize ait güncel fotoğraflar ve bilgiler bu bölümde düzenli olarak paylaşılacaktır.</li>`;

        this.settingsForm.patchValue({
          aboutTitle: settings?.aboutTitle || defaultAboutTitle,
          aboutDescription: settings?.aboutDescription || defaultAboutDescription,
          activityRegionText: settings?.activityRegionText || defaultActivityRegionText,
          instagramUrl: settings?.instagramUrl || '',
          address: settings?.address || '',
          phone: settings?.phone || '',
          email: settings?.email || ''
        });
        this.isLoading = false;
      },
      error: (err: any) => {
        console.error('Settings load error:', err);
        this.errorMessage = 'Ayarlar yüklenirken bir hata oluştu.';
        this.isLoading = false;
      }
    });
  }

  saveSettings(): void {
    if (this.settingsForm.invalid) return;

    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    const updatedSettings: SiteSetting = {
      ...this.currentSettings,
      ...this.settingsForm.value
    };

    this.settingsService.updateSettings(updatedSettings).subscribe({
      next: () => {
        this.successMessage = 'Ayarlar başarıyla kaydedildi.';
        this.isSaving = false;
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (err: any) => {
        console.error('Settings save error:', err);
        this.errorMessage = 'Ayarlar kaydedilirken bir hata oluştu.';
        this.isSaving = false;
      }
    });
  }
}
