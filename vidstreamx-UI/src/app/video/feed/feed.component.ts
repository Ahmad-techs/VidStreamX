import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { VideoService } from 'src/app/services/video.service';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {

  videos: any[] = [];
  loading = false;

  backendUrl = 'https://localhost:7074';

  constructor(private vs: VideoService, private snack: MatSnackBar) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    this.vs.getFeed().subscribe({
      next: v => {
        this.videos = v.map(video => ({
          ...video,
          blobUrl: video.blobUrl.startsWith('http') ? video.blobUrl : `${this.backendUrl}${video.blobUrl}`
        }));
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.snack.open('Failed to load feed', 'Close', { duration: 2000 });
      }
    });
  }

like(video: any): void {
  const user = JSON.parse(localStorage.getItem('vid_user') || '{}');
  const userId = user.userId || user.id;

  if (!userId || !user.token) {
    this.snack.open('Please login', 'Close', { duration: 2000 });
    return;
  }

  this.vs.like(video.id, userId).subscribe({
    next: (res: any) => {
      this.snack.open('Liked', 'Close', { duration: 1000 });
      video.likesCount = res.likesCount;
    },
    error: (err: any) => {
      console.error(err);
      const msg = err?.error?.Message || err?.error || 'Failed to like';
      this.snack.open(msg, 'Close', { duration: 2000 });
    }
  });
}

}
