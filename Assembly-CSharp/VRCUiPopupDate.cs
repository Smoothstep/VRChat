using System;
using System.Collections.Generic;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C84 RID: 3204
public class VRCUiPopupDate : VRCUiPopupStandard
{
	// Token: 0x0600635E RID: 25438 RVA: 0x00234E74 File Offset: 0x00233274
	public override void Start()
	{
		this.monthSpin = base.transform.Find("MonthSpinner").GetComponentInChildren<Spinner>();
		this.monthSpecialText = this.monthSpin.transform.Find("SpecialFormatText").GetComponent<Text>();
		this.dayTenSpin = base.transform.Find("DayTenSpinner").GetComponentInChildren<Spinner>();
		this.dayOneSpin = base.transform.Find("DayOneSpinner").GetComponentInChildren<Spinner>();
		this.yearCentSpin = base.transform.Find("YearCenturySpinner").GetComponentInChildren<Spinner>();
		this.centurySpecialText = this.yearCentSpin.transform.Find("SpecialFormatText").GetComponent<Text>();
		this.yearTenSpin = base.transform.Find("YearTenSpinner").GetComponentInChildren<Spinner>();
		this.yearOneSpin = base.transform.Find("YearOneSpinner").GetComponentInChildren<Spinner>();
		this.monthSpin.Min = 1;
		this.monthSpin.Max = 12;
		this.dayTenSpin.Min = 0;
		this.dayTenSpin.Max = 3;
		this.dayOneSpin.Min = 0;
		this.dayOneSpin.Max = 9;
		this.yearCentSpin.Min = 1;
		this.yearCentSpin.Max = 20;
		this.yearTenSpin.Min = 0;
		this.yearTenSpin.Max = 9;
		this.yearOneSpin.Min = 0;
		this.yearOneSpin.Max = 9;
		this.RegisterValidatorCallbacks();
		this.SetFieldText(2017, 1, 1);
	}

	// Token: 0x0600635F RID: 25439 RVA: 0x0023500C File Offset: 0x0023340C
	private void RegisterValidatorCallbacks()
	{
		this.monthSpin.onValueChanged.AddListener(new UnityAction<string>(this.ValidateMonth));
		this.dayTenSpin.onValueChanged.AddListener(new UnityAction<string>(this.ValidateDayTen));
		this.dayOneSpin.onValueChanged.AddListener(new UnityAction<string>(this.ValidateDayOne));
		this.yearCentSpin.onValueChanged.AddListener(new UnityAction<string>(this.ValidateYearCent));
		this.yearTenSpin.onValueChanged.AddListener(new UnityAction<string>(this.ValidateYearTen));
		this.yearOneSpin.onValueChanged.AddListener(new UnityAction<string>(this.ValidateYearOne));
	}

	// Token: 0x06006360 RID: 25440 RVA: 0x002350C4 File Offset: 0x002334C4
	private void DisableValidatorCallbacks()
	{
		this.monthSpin.onValueChanged.RemoveAllListeners();
		this.dayTenSpin.onValueChanged.RemoveAllListeners();
		this.dayOneSpin.onValueChanged.RemoveAllListeners();
		this.yearCentSpin.onValueChanged.RemoveAllListeners();
		this.yearTenSpin.onValueChanged.RemoveAllListeners();
		this.yearOneSpin.onValueChanged.RemoveAllListeners();
	}

	// Token: 0x06006361 RID: 25441 RVA: 0x00235134 File Offset: 0x00233534
	private void SetFieldText(int year, int month, int day)
	{
		this.inputFieldValue = string.Format("{0:00}/{1:00}/{2:0000}", month, day, year);
		this.monthSpecialText.text = this.monthStrings[month - 1];
		int num = year / 1000;
		int num2 = (year - 1000 * num) / 100;
		this.centurySpecialText.text = string.Format("{0:0}     {1:0}", num, num2);
		this.DisableValidatorCallbacks();
		this.monthSpin.Value = month;
		this.dayTenSpin.Value = day / 10;
		this.dayOneSpin.Value = day % 10;
		this.yearCentSpin.Value = year / 100;
		this.yearTenSpin.Value = year % 100 / 10;
		this.yearOneSpin.Value = year % 10;
		this.RegisterValidatorCallbacks();
	}

	// Token: 0x06006362 RID: 25442 RVA: 0x00235214 File Offset: 0x00233614
	private int ValidateDaysInMonth(int year, int month, int day)
	{
		int num = DateTime.DaysInMonth(year, month);
		if (day > num)
		{
			day = num;
		}
		return day;
	}

	// Token: 0x06006363 RID: 25443 RVA: 0x00235234 File Offset: 0x00233634
	private void ParseSpinners(out int year, out int month, out int day)
	{
		month = this.monthSpin.Value;
		day = this.dayTenSpin.Value * 10 + this.dayOneSpin.Value;
		year = this.yearCentSpin.Value * 100 + this.yearTenSpin.Value * 10 + this.yearOneSpin.Value;
	}

	// Token: 0x06006364 RID: 25444 RVA: 0x00235298 File Offset: 0x00233698
	private void ValidateMonth(string newMonth)
	{
		int year;
		int month;
		int day;
		this.ParseSpinners(out year, out month, out day);
		day = this.ValidateDaysInMonth(year, month, day);
		this.SetFieldText(year, month, day);
	}

	// Token: 0x06006365 RID: 25445 RVA: 0x002352C4 File Offset: 0x002336C4
	private void ValidateDayTen(string ten)
	{
		this.ValidateDay(int.Parse(ten), this.dayOneSpin.Value);
	}

	// Token: 0x06006366 RID: 25446 RVA: 0x002352DD File Offset: 0x002336DD
	private void ValidateDayOne(string one)
	{
		this.ValidateDay(this.dayTenSpin.Value, int.Parse(one));
	}

	// Token: 0x06006367 RID: 25447 RVA: 0x002352F8 File Offset: 0x002336F8
	private void ValidateDay(int newTen, int newOne)
	{
		int year;
		int month;
		int num;
		this.ParseSpinners(out year, out month, out num);
		if (num < 1)
		{
			num = 1;
		}
		num = this.ValidateDaysInMonth(year, month, num);
		this.SetFieldText(year, month, num);
	}

	// Token: 0x06006368 RID: 25448 RVA: 0x0023532D File Offset: 0x0023372D
	private void ValidateYearCent(string cent)
	{
		this.ValidateYear(int.Parse(cent), this.yearTenSpin.Value, this.yearOneSpin.Value);
	}

	// Token: 0x06006369 RID: 25449 RVA: 0x00235351 File Offset: 0x00233751
	private void ValidateYearTen(string ten)
	{
		this.ValidateYear(this.yearCentSpin.Value, int.Parse(ten), this.yearOneSpin.Value);
	}

	// Token: 0x0600636A RID: 25450 RVA: 0x00235375 File Offset: 0x00233775
	private void ValidateYearOne(string one)
	{
		this.ValidateYear(this.yearCentSpin.Value, this.yearTenSpin.Value, int.Parse(one));
	}

	// Token: 0x0600636B RID: 25451 RVA: 0x0023539C File Offset: 0x0023379C
	private void ValidateYear(int newCent, int newTen, int newOne)
	{
		int num;
		int month;
		int day;
		this.ParseSpinners(out num, out month, out day);
		if (num < 1901)
		{
			num = 1901;
		}
		if (num > 2017)
		{
			num = 2017;
		}
		day = this.ValidateDaysInMonth(num, month, day);
		this.SetFieldText(num, month, day);
	}

	// Token: 0x0600636C RID: 25452 RVA: 0x002353EA File Offset: 0x002337EA
	public override void Initialize(string title, string body)
	{
		base.Initialize(title, string.Empty);
		this.mHidePopupOnSubmit = false;
		this.nextInputField = null;
		this.showNextInputFieldOnDisable = false;
	}

	// Token: 0x0600636D RID: 25453 RVA: 0x0023540D File Offset: 0x0023380D
	public void SetupInputAndSubmitButton(string placeholderText, InputField.InputType inputType, string buttonText, Action<string, List<KeyCode>, Text> onSubmit, bool hidePopupOnSubmit = true)
	{
		this.mOnSubmitDelegate = onSubmit;
		base.SetupMiddleButton(buttonText, new Action(this.CloseAndSubmit));
		this.mHidePopupOnSubmit = hidePopupOnSubmit;
	}

	// Token: 0x0600636E RID: 25454 RVA: 0x00235434 File Offset: 0x00233834
	public void SetupInputAndSubmitAndCancelButton(string placeholderText, InputField.InputType inputType, string submitText, Action<string, List<KeyCode>, Text> onSubmit, string cancelText, Action onCancel, bool hidePopupOnSubmit = true)
	{
		this.mOnSubmitDelegate = onSubmit;
		base.SetupRightButton(submitText, new Action(this.CloseAndSubmit));
		this.mOnCancelDelegate = onCancel;
		base.SetupLeftButton(cancelText, new Action(this.CloseAndCancel));
		this.mHidePopupOnSubmit = hidePopupOnSubmit;
	}

	// Token: 0x0600636F RID: 25455 RVA: 0x00235480 File Offset: 0x00233880
	private void CloseAndSubmit()
	{
		if (this.mOnSubmitDelegate != null)
		{
			this.mOnSubmitDelegate(this.inputFieldValue, null, this.popupTitleText);
		}
		if (this.nextInputField != null)
		{
			this.FlagShowNextInput();
		}
		else if (this.mHidePopupOnSubmit)
		{
			VRCUiManager.Instance.popups.HideCurrentPopup();
		}
	}

	// Token: 0x06006370 RID: 25456 RVA: 0x002354E6 File Offset: 0x002338E6
	private void FlagShowNextInput()
	{
		VRCUiManager.Instance.popups.HideCurrentPopup();
		this.showNextInputFieldOnDisable = true;
	}

	// Token: 0x06006371 RID: 25457 RVA: 0x002354FE File Offset: 0x002338FE
	private void ShowNextInput()
	{
		if (this.nextInputField != null)
		{
			this.nextInputField.PressEdit();
		}
	}

	// Token: 0x06006372 RID: 25458 RVA: 0x0023551C File Offset: 0x0023391C
	private void CloseAndCancel()
	{
		if (this.mOnCancelDelegate != null)
		{
			this.mOnCancelDelegate();
		}
		VRCUiManager.Instance.popups.HideCurrentPopup();
	}

	// Token: 0x06006373 RID: 25459 RVA: 0x00235543 File Offset: 0x00233943
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x06006374 RID: 25460 RVA: 0x0023554B File Offset: 0x0023394B
	public override void OnDisable()
	{
		base.OnDisable();
		if (this.showNextInputFieldOnDisable)
		{
			base.Invoke("ShowNextInput", 0.01f);
		}
	}

	// Token: 0x040048C7 RID: 18631
	public UiInputField nextInputField;

	// Token: 0x040048C8 RID: 18632
	private Text mPopupButtonSubmitText;

	// Token: 0x040048C9 RID: 18633
	public string inputFieldValue;

	// Token: 0x040048CA RID: 18634
	private Action<string, List<KeyCode>, Text> mOnSubmitDelegate;

	// Token: 0x040048CB RID: 18635
	private Action mOnCancelDelegate;

	// Token: 0x040048CC RID: 18636
	private bool mHidePopupOnSubmit;

	// Token: 0x040048CD RID: 18637
	private bool showNextInputFieldOnDisable;

	// Token: 0x040048CE RID: 18638
	private Spinner monthSpin;

	// Token: 0x040048CF RID: 18639
	private Spinner dayTenSpin;

	// Token: 0x040048D0 RID: 18640
	private Spinner dayOneSpin;

	// Token: 0x040048D1 RID: 18641
	private Spinner yearCentSpin;

	// Token: 0x040048D2 RID: 18642
	private Spinner yearTenSpin;

	// Token: 0x040048D3 RID: 18643
	private Spinner yearOneSpin;

	// Token: 0x040048D4 RID: 18644
	private Text monthSpecialText;

	// Token: 0x040048D5 RID: 18645
	private Text centurySpecialText;

	// Token: 0x040048D6 RID: 18646
	private const int MIN_YEAR = 1901;

	// Token: 0x040048D7 RID: 18647
	private const int MAX_YEAR = 2017;

	// Token: 0x040048D8 RID: 18648
	private readonly string[] monthStrings = new string[]
	{
		"1-JAN",
		"2-FEB",
		"3-MAR",
		"4-APR",
		"5-MAY",
		"6-JUN",
		"7-JUL",
		"8-AUG",
		"9-SEP",
		"10-OCT",
		"11-NOV",
		"12-DEC"
	};
}
