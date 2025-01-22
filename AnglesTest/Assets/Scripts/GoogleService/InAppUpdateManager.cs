using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.AppUpdate;
using Google.Play.Common;
using System;

public class InAppUpdateManager : MonoBehaviour
{
    AppUpdateManager _appUpdateManager;

    public void Initialize(Action<string> NotUpdate)
    {
#if UNITY_EDITOR
        NotUpdate?.Invoke("에디터 실행");
#elif UNITY_ANDROID
        _appUpdateManager = new AppUpdateManager();
        StartCoroutine(CheckForUpdate(NotUpdate));
#endif
    }

    IEnumerator CheckForUpdate(Action<string> NotUpdate)
    {
        yield return new WaitForSeconds(1);

        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation = _appUpdateManager.GetAppUpdateInfo();

        // Wait until the asynchronous operation completes.
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
                var startUpdateRequest = _appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);

                while (startUpdateRequest.IsDone == false)
                {
                    if (startUpdateRequest.Status == AppUpdateStatus.Downloading)
                    {
                        //Debug.Log("업데이트 다운로드 진행 중");
                    }
                    else if (startUpdateRequest.Status == AppUpdateStatus.Downloaded)
                    {
                        //Debug.Log("다운로드 완료");
                    }

                    yield return null;
                }

                var result = _appUpdateManager.CompleteUpdate();
                while (result.IsDone == false)
                {
                    yield return new WaitForEndOfFrame();
                }

                yield return (int)startUpdateRequest.Status;
            }
            else if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateNotAvailable)
            {
                Debug.Log("업데이트가 없음");
                NotUpdate?.Invoke("업데이트가 없음");
                yield return (int)UpdateAvailability.UpdateNotAvailable;
            }
            else
            {
                Debug.Log("업데이트 가능 여부를 알 수 없음");
                NotUpdate?.Invoke("업데이트 가능 여부를 알 수 없음");
                yield return (int)UpdateAvailability.Unknown;
            }
        }
        else
        {
            // Log appUpdateInfoOperation.Error.
            Debug.Log("업데이트 오류");
            NotUpdate?.Invoke("업데이트 오류");
        }
    }
}
