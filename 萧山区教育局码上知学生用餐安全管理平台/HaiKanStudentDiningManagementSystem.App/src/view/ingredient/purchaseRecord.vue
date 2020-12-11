<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.purchaseRecord.query.totalCount"
        :pageSize="stores.purchaseRecord.query.pageSize"
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
                      v-model="stores.purchaseRecord.query.kw"
                      placeholder="输入食材名称搜索..."
                      @on-search="handleSearchPurchaseRecord()"
                    />
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      v-model="stores.purchaseRecord.query.kw2"
                      type="daterange"
                      placeholder="采购时间"
                      format="yyyy/MM/dd"
                      style="width: 200px"
                      @on-change="dateChage"
                    ></DatePicker>
                  </FormItem>
                  <FormItem>
                    <Select v-model="stores.purchaseRecord.query.kw3"
                     style="width: 200px" 
                     clearable 
                     placeholder="请选择采购方"
                     @on-change="handleRefresh()">
                      <Option
                        v-for="item in canteenList"
                        :value="item.value"
                        :key="item.value"
                        >{{ item.label }}</Option
                      >
                    </Select>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <!-- <Button
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('recover')"
                  ></Button>-->
                  <!--                  <Button-->
                  <!--                    class="txt-danger"-->
                  <!--                    icon="md-hand"-->
                  <!--                    title="禁用"-->
                  <!--                    @click="handleBatchCommand('forbidden')"-->
                  <!--                  ></Button>-->
                  <!--                  <Button-->
                  <!--                    class="txt-success"-->
                  <!--                    icon="md-checkmark"-->
                  <!--                    title="启用"-->
                  <!--                    @click="handleBatchCommand('normal')"-->
                  <!--                  ></Button>-->
                  <Button
                    icon="md-refresh"
                    title="刷新"
                    @click="handleRefresh"
                  ></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增食材"
                  >新增食材</Button
                >
                <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportCuisine"
                  title="导入"
                  >导入</Button
                >
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
          :data="stores.purchaseRecord.data"
          :columns="stores.purchaseRecord.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <!-- <template slot-scope="{row,index}" slot="userType">
            <span>{{renderUserType(row.userType)}}</span>
          </template>-->
          <template slot-scope="{ row, index }" slot="status">
            <Tag :color="renderStatus(row.state).color">{{
              renderStatus(row.state).text
            }}</Tag>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <Poptip
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
            >
              <Tooltip
                placement="top"
                content="删除"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  v-can="'delete'"
                  type="error"
                  size="small"
                  shape="circle"
                  icon="md-trash"
                ></Button>
              </Tooltip>
            </Poptip>
            <Tooltip
              placement="top"
              content="编辑"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-if="row.state == '0'"
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip
              placement="top"
              content="详情"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleShow(row)"
              ></Button>
            </Tooltip>
            <!--            <Tooltip placement="top" content="分配角色" :delay="1000" :transfer="true">-->
            <!--              <Button type="success" size="small" shape="circle" icon="md-contacts" @click="handleAssignRole(row)"></Button>-->
            <!--            </Tooltip>-->
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form
        :model="formModel.fields"
        ref="formPurchaseRecord"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="食材名称" prop="foodName">
              <Input
                :readonly="checkShow()"
                v-model="formModel.fields.foodName"
                placeholder="请输入食材名称"
                @on-change="getIngredient()"
                style="width: 350px"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="食材类型" prop="typeUuid">
              <Select
                v-model="formModel.fields.typeUuid"
                :disabled="true"
                style="width: 350px"
              >
                <Option
                  v-for="item in stores.purchaseRecord.sources.types2"
                  :value="item.typeUuid"
                  :key="item.typeUuid"
                  >{{ item.name }}</Option
                >
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="供应商" prop="supplier">
              <Input
                :readonly="checkShow()"
                v-model="formModel.fields.supplier"
                placeholder="请输入供应商"
                style="width: 350px"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="采购时间" prop="purchaseDate">
              <DatePicker
                v-model="formModel.fields.purchaseDate"
                type="datetime"
                format="yyyy-MM-dd HH:mm"
                placeholder="采购时间"
                style="width: 350px"
                @on-change="dateChage"
                :readonly="checkShow()"
              ></DatePicker>
            </FormItem>
          </Col>
        </Row>

        <Row :gutter="16">
          <Col span="14">
            <FormItem label="采购数量" prop="purchaseNum">
              <Input
                v-model="formModel.fields.purchaseNum"
                placeholder="请输入采购数量"
                style="width: 350px"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="单位" prop="unit">
              <Input
                :readonly="checkShow()"
                v-model="formModel.fields.unit"
                placeholder="请输入单位"
                style="width: 350px"
              />
            </FormItem>
          </Col>
        </Row>

        <Row :gutter="16">
          <Col span="14">
            <FormItem label="采购人(中文，隔开)" prop="systemUserUuid">
              <Input
                :readonly="checkShow()"
                v-model="formModel.fields.systemUserUuid"
                placeholder="请输入采购人"
                style="width: 350px"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="单价(元/斤)" prop="price">
              <InputNumber
                class="inputnum"
                v-model="formModel.fields.price"
                placeholder="请输入单价"
                style="width: 350px"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="热能(kcal/100g)" prop="heatEnergy">
              <InputNumber
                :readonly="numIsReadonly"
                class="inputnum"
                v-model="formModel.fields.heatEnergy"
                placeholder="请输入食材热量"
                style="width: 350px"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="蛋白质(g/100g)" prop="protein">
              <InputNumber
                :readonly="numIsReadonly"
                class="inputnum"
                v-model="formModel.fields.protein"
                placeholder="请输入食材蛋白质含量"
                style="width: 350px"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="脂肪(g/100g)" prop="fat">
              <InputNumber
                :readonly="numIsReadonly"
                class="inputnum"
                v-model="formModel.fields.fat"
                placeholder="请输入食材名称"
                style="width: 350px"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="糖类(g/100g)" prop="saccharides">
              <InputNumber
                :readonly="numIsReadonly"
                class="inputnum"
                v-model="formModel.fields.saccharides"
                placeholder="请输入食材名称"
                style="width: 350px"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="VA(g/100g)" prop="va">
              <InputNumber
                :readonly="numIsReadonly"
                class="inputnum"
                v-model="formModel.fields.va"
                placeholder="请输入食材名称"
                style="width: 350px"
              />
            </FormItem>
          </Col>
        </Row>
        <Row>
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon
                  type="ios-eye-outline"
                  style="float: left"
                  @click.native="handleView(item.url)"
                ></Icon>
                <Icon
                  v-if="isshowdelete"
                  type="ios-trash-outline"
                  style="float: right"
                  @click.native="handleRemove(item.name)"
                ></Icon>
              </div>
            </template>
            <template v-else>
              <Progress
                v-if="item.showProgress"
                :percent="item.percentage"
                hide-info
              ></Progress>
            </template>
          </div>
          <Divider dashed />
          <Upload
            v-show="!checkShow()"
            v-if="isshowup"
            ref="upload"
            :show-upload-list="false"
            :default-file-list="defaultList"
            :on-success="showUpResult"
            :on-progress="toUpResult"
            :format="['jpg', 'jpeg', 'png']"
            :max-size="5120"
            :data="{ uuid: this.$store.state.user.userGuid }"
            :on-format-error="handleFormatError"
            :on-exceeded-size="handleMaxSize"
            :before-upload="handleBeforeUpload"
            type="drag"
            :action="actionurl"
            :headers="postheaders"
            style="display: inline-block; width: 58px"
          >
            <div style="width: 58px; height: 58px; line-height: 58px">
              <Icon type="ios-camera" size="20"></Icon>
            </div>
          </Upload>
          <Modal title="查看图片" v-model="visible">
            <img :src="imgName" v-if="visible" style="width: 100%" />
          </Modal>
        </Row>
      </Form>
      <div v-if="!checkShow()" class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitPurchaseRecord(0)"
          >保 存</Button
        >
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitPurchaseRecord(1)"
          >提 交</Button
        >
        <Button
          style="margin-left: 8px"
          icon="md-close"
          @click="formModel.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
    <Drawer
      title="食材采购记录导入"
      v-model="formimport.opened"
      width="600"
      :mask-closable="true"
      :mask="true"
    >
      <div>
        <input
          ref="inputer"
          id="upload"
          type="file"
          @change="importfxx"
          accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
        />
        <Button
          v-can="'template'"
          icon="ios-cloud-download"
          type="warning"
          @click="handleimportmodel"
          title="食材采购记录导入模板下载"
          >食材采购记录导入模板下载</Button
        >
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleimport"
          :disabled="importdisable"
          >导入</Button
        >

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
import {
  getPurchaseRecordList,
  createPurchaseRecord,
  loadPurchaseRecord,
  editPurchaseRecord,
  deletePurchaseRecord,
  batchCommand,
  deletetoFile,
  loadIngredient,
  GetImport,
  GetExportPass,
  GetGetCanteen
} from "@/api/ingredient/purchaseRecord";
import {
  loadIngredient2,
  loadIngredient3,
  GetFoodTypeList,
} from "@/api/ingredient/ingredient";
import { getStaffList } from "@/api/rbac/user";
import {
  globalvalidateIDCardNoMust,
  globalvalidatepwd,
  globalvalidateIsNotEmpty,
  globalvalidateNumIsNotEmpty,
} from "@/global/validate";
import config from "@/config";
import { getToken } from "@/libs/util";
export default {
  name: "purchaseRecord",
  components: {
    DzTable,
  },
  data() {
    return {
      url: config.baseUrl.dev,
      importdisable: false,
      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false,
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        Import: { name: "Import", title: "导入" },
        // forbidden: { name: "forbidden", title: "禁用" },
        // normal: { name: "normal", title: "启用" }
      },
      num: 0,
      imgshow1: false,
      img1: "",
      img: [],
      url: config.baseUrl.dev,
      imgName: "",
      visible: false,
      updisabled: false,
      uploadList: [],
      defaultList: [],
      loadingStatus: false,
      actionurl: "",
      postheaders: "",
      numIsReadonly: false,
      isshowup: true,
      isshowdelete: true,
      formModel: {
        opened: false,
        title: "创建用户",
        mode: "create",
        selection: [],
        selectPeople: [],
        fields: {
          purchaseUuid: "",
          ingredientUuid: "",
          foodName: "",
          typeUuid: "",
          supplier: "",
          purchaseDate: "",
          purchaseNum: '',
          heatEnergy: 0,
          protein: 0,
          fat: 0,
          saccharides: 0,
          va: 0,
          addTime: "",
          addPeople: "",
          isDelete: "",
          schoolUuid: "",
          state: "",
          systemUserUuid: "",
          accessory: "",
          price: 0,
          schoolName: "",
          unit: "",
        },
        rules: {
          foodName: [
            {
              validator: globalvalidateIsNotEmpty,
              type: "string",
              required: true,
              message: "请输入食材名称",
            },
          ],
          purchaseDate: [
            {
              type: "date",
              required: true,
              message: "选择时间",
              trigger: "change",
            },
          ],
          typeUuid: [{ required: true, message: "请选择类型" }],
          systemUserUuid: [{ required: true, message: "请选择人员" }],
          price: [
            {
              validator: globalvalidateNumIsNotEmpty,
              type: "number",
              message: "请输入数字",
            },
          ],
          heatEnergy: [
            {
              validator: globalvalidateNumIsNotEmpty,
              type: "number",
              message: "请输入数字",
            },
          ],
          protein: [
            {
              validator: globalvalidateNumIsNotEmpty,
              type: "number",
              message: "请输入数字",
            },
          ],
          fat: [
            {
              validator: globalvalidateNumIsNotEmpty,
              type: "number",
              message: "请输入数字",
            },
          ],
          saccharides: [
            {
              validator: globalvalidateNumIsNotEmpty,
              type: "number",
              message: "请输入数字",
            },
          ],
          va: [
            {
              validator: globalvalidateNumIsNotEmpty,
              type: "number",
              message: "请输入数字",
            },
          ],
        },
      },
      formAssignRole: {
        userGuid: "",
        opened: false,
        ownedRoles: [],
        inited: false,
        roles: [],
      },
      stores: {
        staffList: [],
        purchaseRecord: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw2: "",
            kw3:"",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
            types: [],
            types2: [],
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "原料名称", key: "foodName", sortable: true },
            { title: "食材类型", key: "type", ellipsis: true, tooltip: true },
            { title: "供应商", key: "supplier", ellipsis: true, tooltip: true },
            {
              title: "采购日期",
              key: "purchaseDate",
              ellipsis: true,
              tooltip: true,
            },
            { title: "采购数量", key: "purchaseNum" },
            {
              title: "采购方",
              key: "systemUserUuid",
              ellipsis: true,
              tooltip: true,
            },
            { title: "状态", key: "state", slot: "status" },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static",
      },
      rolelist: [],
      departmentlist: [],
      schoolList: [],
      canteenList: [
      ],
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建采购记录";
      }
      if (this.formModel.mode === "edit") {
        return "编辑采购记录";
      }
      if (this.formModel.mode === "show") {
        let s = "采购记录详情";
        if (
          this.formModel.fields.schoolName != "" &&
          this.$store.state.user.schoolguid == null
        ) {
          s += "   学校:(" + this.formModel.fields.schoolName + ")";
        }
        return s;
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.purchaseUuid);
    },
  },
  methods: {
    loadPurchaseRecordList() {
      //console.log(this.stores.purchaseRecord.query.kw2);
      getPurchaseRecordList(this.stores.purchaseRecord.query).then((res) => {
        //console.log(res.data.data);
        this.stores.purchaseRecord.data = res.data.data;
        this.stores.purchaseRecord.query.totalCount = res.data.totalCount;
      });
    },

    doGetFoodTypeList() {
      GetFoodTypeList().then((res) => {
        console.log(res);
        this.stores.purchaseRecord.sources.types = res.data.data;
        this.stores.purchaseRecord.sources.types2 = res.data.data;
      });
    },
    doGetGetCanteen(){
      GetGetCanteen().then((res) => {
        this.canteenList=res.data.data;
      });
    },
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
      this.isshowup = true;
      this.isshowdelete = true;
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.isshowup = true;
      this.isshowdelete = true;
      this.handleOpenFormWindow();
    },
    handleSwitchFormModeToShow() {
      this.formModel.mode = "show";
      this.isshowup = false;
      this.isshowdelete = false;
      this.handleOpenFormWindow();
    },
    handleEdit(row) {
      this.num = 1;
      this.numIsReadonly = true;
      //this.loadStaffList();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormPurchaseRecord();
      this.doLoadPurchaseRecord(row.purchaseUuid);
    },
    handleShow(row) {
      this.num = 1;
      this.numIsReadonly = true;
      //this.loadStaffList();
      this.handleSwitchFormModeToShow();
      this.handleResetFormPurchaseRecord();
      this.doLoadPurchaseRecord(row.purchaseUuid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadPurchaseRecordList();
    },
    handleShowCreateWindow() {
      this.uploadList = [];
      this.numIsReadonly = false;
      //this.loadStaffList();
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormPurchaseRecord();
    },
    handleSubmitPurchaseRecord(num) {
      let valid = this.validatePurchaseRecordForm();
      //console.log(valid);
      //consol.log(this.formModel.fields);
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreatePurchaseRecord(num);
        }
        if (this.formModel.mode === "edit") {
          this.doEditPurchaseRecord(num);
        }
      }
    },
    handleResetFormPurchaseRecord() {
      this.$refs["formPurchaseRecord"].resetFields();
      //this.$refs["formPurchaseRecord2"].resetFields();
      this.formModel.fields.heatEnergy = 0;
      this.formModel.fields.protein = 0;
      this.formModel.fields.fat = 0;
      this.formModel.fields.saccharides = 0;
      this.formModel.fields.va = 0;
      this.formModel.fields.accessory = "";
    },
    doCreatePurchaseRecord(num) {
      //consol.log(this.$store.state.user);
      this.formModel.fields.state = num.toString();
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      this.formModel.fields.schoolUuid = this.$store.state.user.schoolguid;
      createPurchaseRecord(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadPurchaseRecordList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditPurchaseRecord(num) {
      this.formModel.fields.state = num.toString();
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      console.log(this.formModel.fields);
      editPurchaseRecord(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadPurchaseRecordList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    validatePurchaseRecordForm() {
      let _valid = false;
      this.$refs["formPurchaseRecord"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
          return _valid;
        } else {
          _valid = true;
          // this.$refs["formPurchaseRecord2"].validate((valid) => {
          //   if (!valid) {
          //     this.$Message.error("请完善表单信息");
          //     return _valid;
          //   } else {
          //     _valid = true;
          //   }
          // });
        }
      });
      return _valid;
    },
    doLoadPurchaseRecord(uuid) {
      loadPurchaseRecord({ guid: uuid }).then((res) => {
        console.log(res);
        this.uploadList = [];
        this.formModel.fields = res.data.data;
        console.log(111111111);
        console.log(this.formModel.fields);
        //this.formModel.selectPeople = res.data.data.systemUserUuid.split(",");
        if (this.formModel.fields.accessory != null) {
          let list = this.formModel.fields.accessory.split(",");
          for (let i = 0; i < list.length; i++) {
            this.uploadList.push({
              url:
                config.baseUrl.dev + "UploadFiles/LiveShotPicture/" + list[i],
              status: "finished",
              name: "UploadFiles/LiveShotPicture/" + list[i],
              fileName: list[i],
            });
          }
        }
        this.getIngredient();
      });
    },
    handleDelete(row) {
      this.doDelete(row.purchaseUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deletePurchaseRecord(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadPurchaseRecordList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
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
        },
      });
    },
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadPurchaseRecordList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchPurchaseRecord() {
      //consol.log(this.stores.purchaseRecord.query.kw2);
      this.loadPurchaseRecordList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.purchaseRecord.query.sort.direction = column.order;
      this.stores.purchaseRecord.query.sort.field = column.key;
      this.loadPurchaseRecordList();
    },
    handlePageChanged(page) {
      this.stores.purchaseRecord.query.currentPage = page;
      this.loadPurchaseRecordList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.purchaseRecord.query.pageSize = pageSize;
      this.loadPurchaseRecordList();
    },
    renderUserType(userType) {
      // console.log(userType);
      var userTypeText = "未知";
      if (userType != null && userType != "") {
        userTypeText = userType;
      }
      // switch (userType) {
      //   case 0:
      //     userTypeText = "超级管理员";
      //     break;
      //   case 1:
      //     userTypeText = "管理员";
      //     break;
      //   case 2:
      //     userTypeText = "普通用户";
      //     break;
      // }
      return userTypeText;
    },
    renderStatus(status) {
      // console.log(status);
      let _status = {
        color: "success",
        text: "已提交",
      };
      switch (status) {
        case "0":
          _status.text = "保存中";
          _status.color = "primary";
          break;
      }
      return _status;
    },
    changePeople(e) {
      //consol.log(e);
      this.formModel.fields.systemUserUuid = e.join();
    },
    loadStaffList() {
      getStaffList().then((res) => {
        //consol.log(res);
        this.stores.staffList = res.data.data;
      });
    },
    dateChage(e) {
      //consol.log(e);
      if (e != null && e.length == 2) {
        this.handleSearchPurchaseRecord();
      }
    },
    checkShow() {
      return this.formModel.mode === "show";
    },
    getIngredient() {
      if (this.formModel.fields.foodName.trim() == "") {
        this.formModel.fields.heatEnergy = 0;
        this.formModel.fields.protein = 0;
        this.formModel.fields.fat = 0;
        this.formModel.fields.saccharides = 0;
        this.formModel.fields.va = 0;
        this.formModel.fields.accessory = "";
        this.uploadList = [];
        this.numIsReadonly = false;
        this.isshowup = true;
        if (!this.checkShow()) {
          this.isshowdelete = true;
        }
        return;
      }
      if (this.num == 1) {
        this.num++;
        loadIngredient2({ name: this.formModel.fields.foodName }).then(
          (res) => {
            //console.log(res.data.data);
            if (res.data.data == null) {
              this.formModel.fields.heatEnergy = 0;
              this.formModel.fields.protein = 0;
              this.formModel.fields.fat = 0;
              this.formModel.fields.saccharides = 0;
              this.formModel.fields.va = 0;
              this.formModel.fields.typeUuid = "";
              this.uploadList = [];
              this.numIsReadonly = false;
              this.isshowup = true;
              if (!this.checkShow()) {
                this.isshowdelete = true;
              }
            } else {
              this.numIsReadonly = true;
              this.formModel.fields.ingredientUuid =
                res.data.data.ingredientUuid;
              this.formModel.fields.heatEnergy = res.data.data.heatEnergy;
              this.formModel.fields.protein = res.data.data.protein;
              this.formModel.fields.fat = res.data.data.fat;
              this.formModel.fields.saccharides = res.data.data.saccharides;
              this.formModel.fields.va = res.data.data.va;
              this.formModel.fields.typeUuid = res.data.data.typeUuid;
              this.uploadList = [];
              if (res.data.data.accessory != null) {
                this.isshowup = false;
                if (!this.checkShow()) {
                  this.isshowdelete = false;
                }
                //console.log(res.data.data.accessory);
                let list = res.data.data.accessory.split(",");
                for (let i = 0; i < list.length; i++) {
                  this.uploadList.push({
                    url:
                      config.baseUrl.dev +
                      "UploadFiles/LiveShotPicture/" +
                      list[i],
                    status: "finished",
                    name: "UploadFiles/LiveShotPicture/" + list[i],
                    fileName: list[i],
                  });
                }
              } else {
                this.uploadList = [];
              }
              this.formModel.fields.accessory = res.data.data.accessory;
            }
            if (this.formModel.mode == "show") {
              this.numIsReadonly = true;
            }
          }
        );
      } else {
        loadIngredient3({ name: this.formModel.fields.foodName }).then(
          (res) => {
            console.log(res.data.data);
            if (res.data.data == null) {
              this.formModel.fields.heatEnergy = 0;
              this.formModel.fields.protein = 0;
              this.formModel.fields.fat = 0;
              this.formModel.fields.saccharides = 0;
              this.formModel.fields.va = 0;
              this.formModel.fields.typeUuid = "";
              this.uploadList = [];
              this.numIsReadonly = false;
              this.isshowup = true;
              if (!this.checkShow()) {
                this.isshowdelete = true;
              }
            } else {
              this.numIsReadonly = true;
              this.formModel.fields.ingredientUuid =
                res.data.data.ingredientUuid;
              this.formModel.fields.heatEnergy = res.data.data.heatEnergy;
              this.formModel.fields.protein = res.data.data.protein;
              this.formModel.fields.fat = res.data.data.fat;
              this.formModel.fields.saccharides = res.data.data.saccharides;
              this.formModel.fields.va = res.data.data.va;
              this.formModel.fields.typeUuid = res.data.data.typeUuid;
              this.uploadList = [];
              if (res.data.data.accessory != null) {
                this.isshowup = false;
                if (!this.checkShow()) {
                  this.isshowdelete = false;
                }
                console.log(res.data.data.accessory);
                let list = res.data.data.accessory.split(",");
                for (let i = 0; i < list.length; i++) {
                  this.uploadList.push({
                    url:
                      config.baseUrl.dev +
                      "UploadFiles/LiveShotPicture/" +
                      list[i],
                    status: "finished",
                    name: "UploadFiles/LiveShotPicture/" + list[i],
                    fileName: list[i],
                  });
                }
              } else {
                this.uploadList = [];
              }
              this.formModel.fields.accessory = res.data.data.accessory;
            }
            if (this.formModel.mode == "show") {
              this.numIsReadonly = true;
            }
          }
        );
      }
    },
    async showUpResult(response, file, fileList) {
      console.log(this.$refs.upload.fileList);
      console.log(1);
      console.log(response);
      console.log(file);
      console.log(fileList);
      this.loadingStatus = false;
      this.updisabled = false;
      if (response.code == "200") {
        this.$Message.success(response.message);
        console.log(this.formModel.fields);
        console.log(this.formModel.fields.accessory);
        if (this.formModel.fields.accessory == null) {
          this.formModel.fields.accessory == "";
        }
        console.log(this.formModel.fields.accessory);

        if (this.formModel.fields.accessory.length > 0) {
          this.formModel.fields.accessory += ",";
        }
        this.formModel.fields.accessory += response.data.fname;
        // this.formModel.dFileName = response.data.paths;

        await this.uploadList.push({
          url: config.baseUrl.dev + response.data.strpath.replace("\\", "/"),
          status: "finished",
          name: response.data.strpath,
          fileName: response.data.fname,
        });

        // console.log(
        //   (this.$refs.upload.fileList[0].name = e.data.dataPath.split("\\")[1])
        // );
        // console.log(this.defaultfilelist);
        // if (this.departmentlist.length >= 1) {
        //   this.defaultfilelist = [
        //     { name: this.formModel.fields.name, url: e.data.path }
        //   ];
        // }
        console.log(this.formModel.dFileName);
      } else {
        this.$Message.warning(response.message);
      }
    },
    toUpResult() {
      console.log(this.$refs.upload.fileList);
      //console.log(this.$refs.upload.fileList);
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
        // this.$refs.upload.clearFiles();
        // this.$refs.upload.push({})
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    handleFormatError(file) {
      this.$Notice.warning({
        title: "The file format is incorrect",
        desc: "文件: " + file.name + " 不是png,jpg",
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "Exceeding file size limit",
        desc: "文件 " + file.name + " 太大,超过5M.",
      });
    },
    handleBeforeUpload() {
      // const check = this.uploadList.length < 5;
      // if (!check) {
      //   this.$Notice.warning({
      //     title: "Up to five pictures can be uploaded."
      //   });
      // }
      // return check;
      return true;
    },
    handleView(name) {
      this.imgName = name;
      this.visible = true;
    },
    handleRemove(file) {
      console.log(file);
      // const fileList = this.$refs.upload.fileList;
      // this.$refs.upload.fileList.splice(fileList.indexOf(file), 1);
      deletetoFile({ path: file }).then((res) => {
        console.log(res);
        if (res.data.data == "200") {
          this.uploadList = this.uploadList.filter((x) => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map((x) => x.fileName)
            .join(",");
        } else {
          this.uploadList = this.uploadList.filter((x) => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map((x) => x.fileName)
            .join(",");
        }
      });
    },
    //导入相关操作
    handleImportCuisine() {
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      this.$refs.inputer.value = "";
      this.isexitexcel = false;
      this.formimport.opened = true;
    },
    //下载模板
    handleimportmodel() {
      // window.location.href =
      //   this.url + "UploadFiles/ImportBuidingModel/适龄青年信息导入模板.xlsx";
      GetExportPass().then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.selection = [];
          window.location.href = config.baseUrl.dev + res.data.data;
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
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
        await GetImport(this.exceldata).then((res) => {
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            this.loadPurchaseRecordList();
          } else {
            this.$Message.warning(res.data.message);
          }
          this.$refs.inputer.value = "";
          this.exceldata = new FormData();
          this.isexitexcel = false;
        });
      } else {
        this.$Message.warning("请选择文件");
      }
      this.importdisable = false;
    },
  },
  mounted() {
    this.doGetFoodTypeList();
    //consol.log(this.$store.state.user);
    this.loadPurchaseRecordList();
    this.doGetGetCanteen();
  },
  created() {
    if (this.$store.state.user.schoolguid == null) {
      this.stores.purchaseRecord.columns.splice(1, 0, {
        title: "学校",
        key: "schoolName",
      });
    }
    this.postheaders = {
      Authorization: "Bearer " + getToken(),
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl =
      config.baseUrl.dev + "api/v1/Ingredient/PurchaseRecord/UpLoad";
  },
};
</script>

<style>
.inputnum {
  width: 300px;
}
.demo-upload-list {
  display: inline-block;
  width: 120px;
  height: 120px;
  text-align: center;
  line-height: 120px;
  border: 1px solid transparent;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
  position: relative;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
  margin-right: 4px;
}
.demo-upload-list img {
  width: 100%;
  height: 100%;
}
.demo-upload-list-cover {
  display: none;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
}
.demo-upload-list:hover .demo-upload-list-cover {
  display: block;
}
.demo-upload-list-cover i {
  color: #fff;
  font-size: 20px;
  cursor: pointer;
  margin: 0 2px;
}
</style>
