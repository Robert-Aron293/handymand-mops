import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../interfaces/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  
  

  userSubject = new Subject();

  private baseUrl: string = environment.baseUrl;
  constructor(private http:HttpClient) {

  }

  private publicHeaders = {
    headers:new HttpHeaders({
      'content-type':'application/json'
    })
  }

  
  getMyuser(){
    return  this.http.get(
      this.baseUrl + "/api/users/myuser"
    );
  }

  getUser(data:any){
    return this.http.post(
      this.baseUrl + "/api/users/getuser",
      data,
      this.publicHeaders
    )
  }


  getMyProfilePicture(){
    return this.http.get(
      this.baseUrl + '/api/users/myuserprofileImage'
    );
  }

  getTestPicture(){
    return this.http.get(
      this.baseUrl + '/api/JobOffer/getimagetest',
      {
        responseType:"blob"
      }
    )
  }

  getUserProfilePicture(data:any){
    return this.http.post(
      this.baseUrl + '/api/users/userprofileImage',
      data
    );
  }

  updateMyUser(data:any){
    var result = this.http.put(
      this.baseUrl + "/api/users/updatemyuser",
      data
    )
    return result;
  }

  testEncryption(data:any){
      return this.http.post(
        this.baseUrl + "/api/users/profile/4",
        data
      )
  }

  getAll(){
    return this.http.get(
      this.baseUrl + '/api/users/allforadmin'
    )
  }

  update(data:any){
    return this.http.put(
      this.baseUrl + '/api/users/update',
      data,
      this.publicHeaders
    )
  }

  block(data:any){
    return this.http.put(
      this.baseUrl + '/api/users/block',
      data,
      this.publicHeaders
    )
  }

  delete(data:any){

    const options = {
      headers: new HttpHeaders({
          'content-Type': 'application/json'
      }),
      body: data
  } 

    return this.http.delete(
      this.baseUrl + '/api/users/delete',
      options
    )
  }


  getUserInfoBar(id:number){
    return this.http.get(
      this.baseUrl + "/api/users/getuserinfobar/" + id,
      this.publicHeaders
    )

  }
  
  getTotalNrOfNotifications(){
    return this.http.get(
      this.baseUrl + "/api/users/notificationsnr",
      this.publicHeaders
    )
  }

  
  viewNotification(idJobOffer:number,idNotificationType:number){
    return this.http.post(
      this.baseUrl + "/api/users/viewnotification/" + idJobOffer +  "/" + idNotificationType,
      this.publicHeaders
    )
  }

  getAllUsers(pageNr:number, noElements:number, filter:any){
    return this.http.post(
      this.baseUrl + '/api/users/all?pageNr=' + pageNr + "&noElements=" + noElements,
      filter,
      this.publicHeaders);
  }

  getTotalLength(filter:any){
    return this.http.post(
      this.baseUrl + '/api/users/total',
      filter,
      this.publicHeaders
    )
  }
}