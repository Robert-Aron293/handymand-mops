import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { LoginComponent } from '../pages/login/login.component';
import { User } from '../interfaces/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private userValue:User | undefined;
  
  private baseUrl: string = environment.baseUrl;
  private publicHeaders = {
    headers:new HttpHeaders({
      'content-type':'application/json'
    })
  }

  private isloggedIn: boolean;

  constructor(private http:HttpClient) { 
    this.isloggedIn = false;
  }

  login(data: any){
    return this.http.post(
      this.baseUrl + "/api/Users/authenticate",
      data,
      this.publicHeaders);

  }

  register(data: User){
    return this.http.post(
      this.baseUrl + "/api/Users/create",
      data,
      this.publicHeaders);
  }


  set(value:string, keyEnv:string, Iv:CryptoJS.lib.WordArray){
    var key = CryptoJS.enc.Utf8.parse(keyEnv);
    var encrypted = CryptoJS.AES.encrypt(value, key,
    {
        keySize: 128 / 8,
        iv: Iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });

    return encrypted.toString();
  }

  get(value:string,keyEnv:string,Iv:CryptoJS.lib.WordArray){
    var key = CryptoJS.enc.Utf8.parse(keyEnv);
    var decrypted = CryptoJS.AES.decrypt(value, key, {
        keySize: 128 / 8,
        iv: Iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });
    return decrypted.toString(CryptoJS.enc.Utf8);

  }

  wordToByteArray(word:number, length:number) {
    var ba = [],
      i,
      xFF = 0xFF;
    if (length > 0)
      ba.push(word >>> 24);
    if (length > 1)
      ba.push((word >>> 16) & xFF);
    if (length > 2)
      ba.push((word >>> 8) & xFF);
    if (length > 3)
      ba.push(word & xFF);
  
    return ba;
  }


  wordArrayToByteArray(wordArray:CryptoJS.lib.WordArray, length:number) {
    let words:number[] = [];

    if (wordArray.hasOwnProperty("sigBytes") && wordArray.hasOwnProperty("words")) {
      length = wordArray.sigBytes;
      words = wordArray.words;
    }
  
    var result = [],
      bytes,
      i = 0;

    while (length > 0) {
      bytes = this.wordToByteArray(words[i], Math.min(4, length));
      length -= bytes.length;
      result.push(bytes);
      i++;
    }
    //return [].concat.apply([], result);
    return result.reduce((accumulator, value) => accumulator.concat(value), []);
  }
}