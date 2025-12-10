import { HttpClient, HttpEvent, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VideoService {

   private apiUrl = 'https://localhost:7074/api/Video';

  constructor(private http: HttpClient) {}

  upload(file: File, title: string, userId: string): Observable<HttpEvent<any>> {
    const form = new FormData();
    form.append('file', file);
    form.append('title', title);
    form.append('userId', userId);

    const req = new HttpRequest('POST', `${this.apiUrl}/upload`, form, {
      reportProgress: true
    });
    return this.http.request(req);
  }

  getFeed() {
    return this.http.get<any[]>(`${this.apiUrl}/feed`);
  }

  like(videoId: string, userId: string) {
    const user = JSON.parse(localStorage.getItem('vid_user') || '{}');
    const token = user?.token;

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    return this.http.post(`${this.apiUrl}/${videoId}/like`, { userId }, { headers });
  }

  getVideos(): Observable<any> {
  return this.http.get(`${this.apiUrl}/all`);
}
getVideoById(id: string) {
  return this.http.get<any>(`${this.apiUrl}/${id}`);
}


}
