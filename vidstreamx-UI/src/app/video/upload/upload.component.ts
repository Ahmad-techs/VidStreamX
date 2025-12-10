import { HttpEventType } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from 'src/app/core/auth.service';
import { VideoService } from 'src/app/services/video.service';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent {

    file?: File;
  title = '';
  progress = 0;

  constructor(private vs: VideoService, private auth: AuthService, private snack: MatSnackBar) {}

  onFile(e: any) { this.file = e.target.files[0]; }

upload(): void {
  const user = this.auth.getUser();
  if (!user?.userId) {
    this.snack.open('Please login', 'Close', { duration: 2000 });
    return;
  }

  this.vs.upload(this.file!, this.title, user.userId as string).subscribe({
    next: ev => {
      if (ev.type === HttpEventType.UploadProgress) {
        this.progress = Math.round((100 * (ev.loaded || 0)) / (ev.total || 1));
      } else if (ev.type === HttpEventType.Response) {
        this.snack.open('Upload complete', 'Close', { duration: 2000 });
        this.progress = 0;
      }
    },
    error: () => {
      this.snack.open('Upload failed', 'Close', { duration: 3000 });
    }
  });
}

}
