import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HasPermissionGuard implements CanActivate {

  constructor(private router:Router){

  }

  parseJwt(token:string|null|undefined) {
    if(token != null && token != undefined){
      var base64Url = token.split('.')[1];
      var base64 = base64Url.replace('-', '+').replace('_', '/');
      return JSON.parse(window.atob(base64));
    }

  }


  //https://jasonwatmore.com/post/2020/09/09/angular-10-role-based-authorization-tutorial-with-example
  //https://www.rdegges.com/2018/please-stop-using-local-storage/
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

      localStorage.removeItem("storedcryptedId");
      localStorage.removeItem("storedBase64Iv");

    var path = route.routeConfig?.path;
    var guestPaths = ["home","joboffers","login","register","profile","freelancers"]
    var userPaths = ["profile","hire","jobofferpage/:id","myprofile","editprofile"];
    var adminPaths = ["dashboard","testjs","users","skills"];

    var token = localStorage.getItem('token');
    if(token == null || token == undefined ){
      if(path != undefined && guestPaths.includes(path)){
        return true;
      }

      this.router.navigate(['/login']);
      return false;
    }
    var id = this.parseJwt(token).id;

    if(id != null && id != '' && id != 0){

      if(path == undefined){
        this.router.navigate(['/notfound']);
      }
      else{
        var role = this.parseJwt(token).role;

        if(role == 0){

          if(adminPaths.includes(path) || userPaths.includes(path) || guestPaths.includes(path)){
            return true;
          }
          else{
            this.router.navigate(['/unauthorized']);
          }
        }

        if(role == 1){

          if(userPaths.includes(path) || guestPaths.includes(path)){
            return true;
          }
          else{
            this.router.navigate(['/unauthorized']);
          }
        }

        this.router.navigate(['/unauthorized']);

      }
    }
    else{
      this.router.navigate(['/login']);
    }

    return true;
  }
  
}