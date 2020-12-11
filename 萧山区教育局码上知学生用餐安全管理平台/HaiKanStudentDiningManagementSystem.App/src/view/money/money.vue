<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.postjobs.query.totalCount"
        :pageSize="stores.postjobs.query.pageSize"
        :currentPage="stores.postjobs.query.currentPage"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.postjobs.query.kw"
                      placeholder="输入食品名称搜索..."
                      @on-search="handleSearchPostjobs()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'batch'"
                    v-show="schoolshow1==1"
                    class="txt-danger"
                    icon="md-trash"
                    title="批量删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  v-show="schoolshow1==1"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加"
                >添加</Button>
                <Button
                  v-can="'import'"
                  v-show="schoolshow1==1"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportCuisine"
                  title="导入"
                >导入</Button>
                <Button
                  v-can="'export'"
                  v-show="schoolshow1==1"
                  icon="ios-cloud-download-outline"
                  type="primary"
                  @click="handleExportCuisine('export')"
                  title="导出"
                >导出</Button>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.postjobs.data"
          :columns="stores.postjobs.columns"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <template slot-scope="{row,index}" slot="releaseState">
            <span>{{renderReleaseState(row.releaseState)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="fullState">
            <span>{{renderFullState(row.fullState)}}</span>
          </template>
          <!--          <template slot-scope="{ row, index }" slot="detail">-->
          <!--            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">-->
          <!--              <Button-->
          <!--                type="primary"-->
          <!--                size="small"-->
          <!--                shape="circle"-->
          <!--                icon="md-search"-->
          <!--                @click="handleShow(row)"-->
          <!--              ></Button>-->
          <!--            </Tooltip>-->
          <!--          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip
                placement="top"
                content="删除"
                :delay="1000"
                :transfer="true"
                v-show="schoolshow1==1"
              >
                <Button v-can="'delete'" type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip
              placement="top"
              content="编辑"
              :delay="1000"
              :transfer="true"
              v-show="schoolshow1==1"
            >
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
      :styles="styles"
    >
      <Form
        v-if="formModel.opened"
        :model="formModel.fields"
        ref="formPlan"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="进货时间" prop="purchaseTime">
              <DatePicker
                type="date"
                :value="formModel.fields.purchaseTime"
                @on-change="formModel.fields.purchaseTime=$event"
                format="yyyy-MM-dd"
                placeholder="选择时间"
                style="width: 270px;"
              ></DatePicker>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="供货商联系方式" prop="phone">
            <Input
              v-model="formModel.fields.phone"
              placeholder="请输入供应商联系方式"
              style="width: 400px"
              :maxlength="50"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="供货商" prop="supplier">
            <Input
              v-model="formModel.fields.supplier"
              placeholder="请输入供应商"
              style="width: 400px"
              :maxlength="50"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="食品名称" prop="cuisineName">
            <Input
              v-model="formModel.fields.cuisineName"
              placeholder="请输入食品名称"
              style="width: 400px"
              :maxlength="50"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="生产日期" prop="producedTime">
              <DatePicker
                type="date"
                :value="formModel.fields.producedTime"
                @on-change="formModel.fields.producedTime=$event"
                format="yyyy-MM-dd"
                placeholder="选择时间"
                style="width: 270px;"
              ></DatePicker>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="规格" prop="specification">
            <Input
              v-model="formModel.fields.specification"
              placeholder="请输入规格"
              style="width: 400px"
              :maxlength="50"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="数量" prop="quantity">
            <Input
              v-model="formModel.fields.quantity"
              placeholder="请输入数量"
              style="width: 400px"
              :maxlength="50"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="保质期" prop="expirationTime">
            <Input
              v-model="formModel.fields.expirationTime"
              placeholder="请输入保质期"
              style="width: 400px"
              :maxlength="50"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="保存方式" prop="rt">
            <Input
              v-model="formModel.fields.rt"
              placeholder="请输入保存方式"
              style="width: 400px"
              :maxlength="50"
            />
          </FormItem>
        </Row>
      </Form>
      <div style="margin-top: 60px">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitPlan">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer title="信息导入" v-model="formimport.opened" width="500" :mask-closable="true" :mask="true">
      <div>
        <input
          ref="inputer"
          id="upload"
          type="file"
          @change="importfxx"
          accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
        />
        <Button
          icon="ios-cloud-download"
          type="warning"
          @click="handleimportmodel"
          title="导入模板下载"
        >导入模板下载</Button>
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleimport"
          :disabled="importdisable"
        >导入</Button>

        <Tabs value="name1">
          <TabPane label="成功" name="name1" v-html="successmsg"></TabPane>
          <TabPane label="重复" name="name2" v-html="repeatmsg"></TabPane>
          <TabPane label="失败" name="name3" v-html="defaultmsg"></TabPane>
        </Tabs>
      </div>
    </Drawer>
  </div>
</template>
<script>
  import DzTable from "_c/tables/dz-table.vue";
  import Tables from "_c/tables";
  import {
 MoneyList,
 Create,
 MoneyEdit,
 Batch,
 Delete,
 MoneyGet,
 Import,
 Export
  } from "@/api/money/money";
  import config from "@/config";
import { getToken } from "@/libs/util";
  export default {
    name: "money_page",
    components: {
      Tables,
      DzTable
    },
    data() {
      return {
        //导入
      url: config.baseUrl.dev,
      importdisable: false,
     schoolshow1:0,
      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false
      },

        showdetails: false,
        details: "",
        commands: {
          delete: { name: "delete", title: "删除" },
          recover: { name: "recover", title: "恢复" },
         Import: { name: "Import", title: "导入" },
        export: { name: "export", title: "导出" }
        },
        formModel: {
          opened: false,
          title: "创建类别",
          mode: "create",
          selection: [],
          fields: {
            schoolUuid:"",
            electronicUuid: "",
            supplier: "",
            phone: "",
            isDelete:0,
            purchaseTime:"",           
            cuisineName:"",
            specification:"",
            expirationTime:"",
            producedTime:"",
            quantity:"",
            rt:"",
            addTime:"",
            
          },
          rules: {
            purchaseTime: [{type: "string",required: true,message: "请输入单位名称"}],
             phone: [{type: "string",required: true,message: "请输入供应商联系方式"}],
            supplier: [{type: "string",required: true,message: "请输入供应商"}],
             cuisineName: [{type: "string",required: true,message: "请输入食材名称"}],
             producedTime: [{type: "string",required: true,message: "请输入生产日期"}],
             specification: [{type: "string",required: true,message: "请输入规格"}],
             quantity: [{type: "string",required: true,message: "请输入数量"}],
             expirationTime: [{type: "string",required: true,message: "请输入保质期"}],
             rt: [{type: "string",required: true,message: "请输入保存方式"}],
          }
        },
        stores: {
          postjobs: {
            query: {
              //d定义11
              schoolguid:"",
              totalCount: 0,
              pageSize: 20,
              currentPage: 1,
              kw: "",
              kw1: "",
              year: "",
              isDeleted: 0,
              status: -1,
              releaseState:-1,
              sort: [
                {
                  direct: "DESC",
                  field: "ID"
                }
              ]
            },
            sources: {
              releaseStateSources: [
                { value: -1, text: "全部" },
                { value: 0, text: "未发布" },
                { value: 1, text: "已发布" }
              ],
              isDeletedSources: [
                { value: -1, text: "全部" },
                { value: 0, text: "正常" },
                { value: 1, text: "已删" }
              ],
              statusSources: [
                { value: -1, text: "全部" },
                { value: 0, text: "禁用" },
                { value: 1, text: "正常" }
              ],
              statusFormSources: [
                { value: 0, text: "禁用" },
                { value: 1, text: "正常" }
              ]
            },
            columns: [
              { type: "selection", width: 50, key: "electronicUuid" },
              { title: "进货时间", key: "purchaseTime"},
              { title: "供货商联系方式", key: "phone"},
              { title: "供货商", key: "supplier" },
              { title: "食品名称", key: "cuisineName" },
              { title: "生产日期", key: "producedTime"},
              { title: "规格", key: "specification"},
              { title: "数量", key: "quantity"},
              { title: "保质期", key: "expirationTime" },
              { title: "保存方式", key: "rt"},
              {
                title: "操作",
                fixed: "right",
                align: "center",
                width: 150,             
                className: "table-command-column",
                slot: "action"
              }
            ],
            data: []
          }
        },
        styles: {
          height: "calc(100% - 55px)",
          overflow: "auto",
          paddingBottom: "53px",
          position: "static"
        },
        initdatacopy: {
            supplier: "",
            phone: "",
            isDelete:0,
            purchaseTime:"",
            cuisineName:"",
            specification:"",
            expirationTime:"",
            producedTime:"",
            quantity:"",
            rt:"",
        }
      };
    },
    computed: {
      formTitle() {
        if (this.formModel.mode === "create") {
          return "添加";
        }
        if (this.formModel.mode === "edit") {
          return "编辑";
        }
        return "";
      },
      selectedRows() {
        return this.formModel.selection;
      },
      selectedRowsId() {
        return this.formModel.selection.map(x => x.electronicUuid);
      }
    },
    methods: {
      renderReleaseState(isEnable){
        let _status = "未知";
        switch(isEnable){
          case 0:
            _status= "未发布";
            break;
          case 1:
            _status= "已发布";
            break;
        }
        return _status;
      },
      renderFullState(isEnable){
        let _status = "未知";
        switch(isEnable){
          case 0:
            _status= "未满";
            break;
          case 1:
            _status= "已满";
            break;
        }
        return _status;
      },
      loadPripostjobsList() {
        if (this.$store.state.user.schoolguid == null) {
                this.schoolshow1=0;
                
      }else{
         this.schoolshow1=1;
      }
      //赋值222
        this.stores.postjobs.query.schoolguid = this.$store.state.user.schoolguid;
        console.log("asjghcjasvjhb就撒感激不尽vjhvh");
        console.log(this.stores.postjobs.query.schoolguid);
        MoneyList(this.stores.postjobs.query).then(res => {
          this.stores.postjobs.data = res.data.data;
          this.stores.postjobs.query.totalCount = res.data.totalCount;
          //console.log(this.stores.educaplan.data);
        });
      },
      handleSearchPostjobs() {
        this.stores.postjobs.query.currentPage = 1;
        var schoolUuid = this.$store.state.user.schoolguid;
        this.loadPripostjobsList();
      },
      handleRefresh() {
        this.stores.postjobs.query.currentPage = 1;
        this.loadPripostjobsList();
      },
      //创建，修改(保存按钮)
      handleSubmitPlan() {
      let valid = this.validateForm();
        if (valid) {
          if (this.formModel.mode === "create") {
            //alert(this.formModel.fields.schoolUuid);
            this.docreatePostjobs();
          }
          if (this.formModel.mode === "edit") {
            
            this.doEditPlan();
          }
        }
      },
      docreatePostjobs() {
        this.formModel.fields.schoolUuid=this.$store.state.user.schoolguid;
        Create(this.formModel.fields).then(res => {
          //alert(this.formModel.fields.schoolUuid);
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
             this.formModel.opened = false; //关闭表单
            this.loadPripostjobsList();
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      doEditPlan() {
        MoneyEdit(this.formModel.fields).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
             this.formModel.opened = false; //关闭表单
            this.loadPripostjobsList();
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      validateForm() {
        let _valid = false;
        this.$refs["formPlan"].validate(valid => {
          if (!valid) {
            this.$Message.error("请完善表单信息");
          } else {
            _valid = true;
          }
        });
        return _valid;
      },
      //批量操作
      handleBatchCommand(command) {
        if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        this.$Modal.confirm({
          title: "操作提示",
          content:
            "<p>确定要执行当前 [" +
            this.commands[command].title +
            "] 操作吗?</p>",
          loading: true,
          onOk: () => {
            this.doBatchCommand(command);
          }
        });
        //addsystemlog("delete","删除年度市级招生方案列表");
      },
      doBatchCommand(command) {
        Batch({
          command: command,
          ids: this.selectedRowsId.join(",")
        }).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.loadPripostjobsList();
            this.formModel.selection = [];
          } else {
            this.$Message.warning(res.data.message);
          }
          this.$Modal.remove();
        });
      },
      handleSelectionChange(selection) {
        this.formModel.selection = selection;
      },
      rowClsRender(row, index) {
        if (row.isDeleted) {
          return "table-row-disabled";
        }
        return "";
      },
      //单条删除
      handleDelete(row) {
        this.doDelete(row.electronicUuid);
      },
      doDelete(ids) {
        if (!ids) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        Delete(ids).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.loadPripostjobsList();
            this.formModel.selection = [];
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
         //导入相关操作
    handleImportCuisine() {
      this.importdisable = false;
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      this.$refs.inputer.value = "";
      this.isexitexcel = false;
      this.formimport.opened = true;
    },
     //下载模板
    handleimportmodel() {
      window.location.href =
        this.url + "UploadFiles/ImportModel/电子台账模板.xlsx";
    },
    //导入
    importfxx(e) {
      let inputDOM = this.$refs.inputer;
      //声明一个FormDate对象
      let formData = new FormData();
      let file = inputDOM.files[0];
      let AllUpExt = ".xls|.xlsx|";
      let extName = file.name
        .substring(file.name.lastIndexOf("."))
        .toLowerCase();
      if (AllUpExt.indexOf(extName + "|") == "-1") {
        this.$refs.inputer.value = "";
        this.$Message.warning("文件格式不正确!");
      } else {
        if (file != null && file != undefined) {
          this.isexitexcel = true;
          formData.append("excelFile", file);
          this.exceldata = formData;
        } else {
          this.isexitexcel = false;
        }
      }
    },
    async handleimport() {
      this.importdisable = true;
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      if (this.isexitexcel) {
        await Import(this.exceldata).then(res => {
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            // this.loadDispatchList();
            this.importdisable = false;
          } else {
            this.$Message.warning(res.data.message);
            this.importdisable = false;
          }
          this.$refs.inputer.value = "";
          this.exceldata = new FormData();
          this.isexitexcel = false;
          this.handleRefresh();
        });
      } else {
        this.$Message.warning("请选择文件");
      }
      this.importdisable = false;
    },
    //导出
    handleExportCuisine(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doCuisineExport(command);
        }
      });
    },
    doCuisineExport(command) {
      console.log(this.selectedRowsId.join(","));
      Export(this.selectedRowsId.join(",")).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          //this.loadDispatchList();
          this.formModel.selection = [];
          console.log(res);
          window.location.href = config.baseUrl.dev + res.data.data;
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
      //控制弹出子窗体
      handleOpenFormWindow() {
        this.formModel.opened = true;
      },
      handleCloseFormWindow() {
        this.formModel.opened = false;
      },
      //编辑
      handleEdit(row) {
        this.handleSwitchFormModeToEdit();
        this.handleResetFormRole();

        this.doLoadPostjobs(row.electronicUuid);
      },

      handleShowCreateWindow() {
        //alert(this.$store.state.user.schoolguid);
        //alert(this.formModel.fields.schoolUuid);
        this.handleSwitchFormModeToCreate();
        this.handleResetFormRole();
      },
      handleSwitchFormModeToCreate() {
        this.formModel.mode = "create";
        this.handleOpenFormWindow();
      },
      handleSwitchFormModeToEdit() {
        this.formModel.mode = "edit";
        this.handleOpenFormWindow();
      },
      handleSwitchFormModeToShow() {
        this.showdetails = true;
      },
      handleResetFormRole() {
        this.formModel.fields = JSON.parse(JSON.stringify(this.initdatacopy));
        //this.$refs["formPlan"].resetFields();
      },
      doLoadPostjobs(guid) {
        
      MoneyGet(guid).then(res => {

        if (res.data.code === 200) {
          this.formModel.fields = res.data.data[0];      
        }
      });
    },
      handlePageChanged(page) {
        this.stores.postjobs.query.currentPage = page;
        this.loadPripostjobsList();
      },
      handlePageSizeChanged(pageSize) {
        this.stores.postjobs.query.pageSize = pageSize;
        this.loadPripostjobsList();
      }
    },
    mounted() {
      this.loadPripostjobsList();
    },
    created() {
    this.postheaders = {
      Authorization: "Bearer " + getToken()
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/Cuisine/Cuisine/UpLoad";
  }
  };
</script>
<style scoped>
</style>
